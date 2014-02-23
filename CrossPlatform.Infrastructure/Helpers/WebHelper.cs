using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public class WebHelper : BindableBase
    {
        private static WebHelper _instance;
        /// <summary>
        /// Instance
        /// </summary>
        public static WebHelper Instance
        {
            get
            {
                return _instance = _instance ?? new WebHelper();
            }
        }

        private bool _isBusy;
        /// <summary>
        /// 작업중 여부
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        /// <summary>
        /// Get Html
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> GetHtml(string uri)
        {
            string returnValue = string.Empty;
            IsBusy = true;
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var result = await hc.GetAsync(uri);
                    if (result != null && result.IsSuccessStatusCode == true)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        returnValue = data;
                    }
                }
                catch (Exception)
                {
                }
            }
            IsBusy = false;
            return returnValue;
        }
    }
}
