using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class WebAPIBase<T> : BindableBase
    {
        /// <summary>
        /// 베이스Uri
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// 서비스 Uri
        /// </summary>
        public string ServiceUri { get; set; }

        /// <summary>
        /// 결과코드
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 결과메시지
        /// </summary>
        public string ResultMsg { get; set; }

        /// <summary>
        /// 반환갯수
        /// </summary>
        protected int NumOfRows { get; set; }

        private int pageNo;
        /// <summary>
        /// 페이지 번호
        /// </summary>
        public int PageNo
        {
            get { return pageNo; }
            set
            {
                pageNo = value;
                OnPropertyChanged();
            }
        }

        private int totalCount;
        /// <summary>
        /// 전체 카운트
        /// </summary>
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 토탈 페이지 = totalCount / NumOfRows + 1
        /// </summary>
        public int TotalPage { get; set; }

        private bool isBusy;
        /// <summary>
        /// 작업중 여부
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 생성자
        /// </summary>
        public WebAPIBase()
        {
            BaseUri = "";
            ServiceUri = "";
            NumOfRows = 30;
            PageNo = 1;

            this.PropertyChanged +=
                (s, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "NumOfRows":
                        case "TotalCount":
                            if (TotalCount > 0 && NumOfRows > 0)
                            {
                                TotalPage = TotalCount / NumOfRows;
                                if ((TotalCount % NumOfRows) > 0)
                                    TotalPage++;
                            }
                            else
                            {
                                TotalPage = 0;
                            }
                            break;
                    }
                };
        }

        /// <summary>
        /// 조회
        /// </summary>
        protected async Task<string> GetData(string requestUri) 
        {
            string result = string.Empty;
            IsBusy = true;

            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    //타임아웃30초
                    hc.Timeout = new TimeSpan(0, 0, 30);
                    var uri = string.Format("{0}{1}", BaseUri, requestUri);
                    result = await hc.GetStringAsync(uri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    IsBusy = false;
                }
            }
            IsBusy = false;
            return result;
        }

        /// <summary>
        /// 조회
        /// </summary>
        public abstract Task<T> GetAsync(string para);

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public virtual async Task<bool> PostAsync(T postItem)
        {
            bool returnValue = false;
            if (postItem != null)
            {
                var json = JsonConvert.SerializeObject(postItem);
                if (json != null && json.Length > 0)
                {
                    using (var hc = new HttpClient())
                    {
                        var sc = new StringContent(json, Encoding.UTF8, "application/json");
                        try
                        {
                            var resp = await hc.PostAsync(BaseUri, sc);
                            if (resp.IsSuccessStatusCode == true)
                            {
                                returnValue = true;
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            EtcUtility.Instance.MsgBox("PostAsync, error :" + ex.Message);
#else
                            EtcUtility.Instance.MsgBox("등록 중 오류가 발생했습니다..\n" + ResultMsg);
#endif
                        }
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="putItem"></param>
        /// <returns></returns>
        public virtual async Task<bool> PutAsync(T putItem, object key)
        {
            bool returnValue = false;

            var json = JsonConvert.SerializeObject(putItem);
            if (json != null && json.Length > 0)
            {
                using (var hc = new HttpClient())
                {
                    var sc = new StringContent(json, Encoding.UTF8, "application/json");
                    var uri = string.Format("{0}/{1}", BaseUri, key.ToString());
                    try
                    {
                        var resp = await hc.PutAsync(uri, sc);
                        if (resp.IsSuccessStatusCode == true)
                        {
                            returnValue = true;
                        }
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        EtcUtility.Instance.MsgBox("PutAsync, error :" + ex.Message);
#else
                        EtcUtility.Instance.MsgBox("수정 중 오류가 발생했습니다..\n" + ResultMsg);
#endif
                    }
                }
            }
            return returnValue;

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="deleteItem"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(T deleteItem, object key)
        {
            bool returnValue = false;

            using (var hc = new HttpClient())
            {
                var query = string.Format("{0}/{1}", BaseUri, key.ToString());
                try
                {
                    var resp = await hc.DeleteAsync(query);
                    if (resp.IsSuccessStatusCode)
                    {
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    EtcUtility.Instance.MsgBox("DeleteAsync, error :" + ex.Message);
#else
                    EtcUtility.Instance.MsgBox("삭제 중 오류가 발생했습니다..\n" + ResultMsg);
#endif
                }
            }
            return returnValue;
        }

    }
}
