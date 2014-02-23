using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class EtcUtility
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static EtcUtility Instance { get; set; }

        /// <summary>
        /// MessageBox call method
        /// </summary>
        /// <param name="content"></param>
        public abstract void MsgBox(string content);

        /// <summary>
        /// Confirm call method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public abstract System.Threading.Tasks.Task<bool> ConfirmAsync(string context, string title = "Confirm");

        /// <summary>
        /// Navigated Event handler
        /// </summary>
        //public abstract event EventHandler<NavigationArgs> Navigated;

        /// <summary>
        /// 오브젝트 내부의 프로퍼티 데이터를 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public abstract T GetPropertyValue<T>(object source, string propertyName) where T : class;

        /// <summary>
        /// 오브젝트 내부의 프로퍼티를 오브젝트로 반환
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public abstract object GetPropertyValue(object source, string propertyName);

        /// <summary>
        /// 오브젝트가 해당 프로퍼티를 가지고 있는지 확인
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public abstract bool IsExistProperty(object source, string propertyName);

        /// <summary>
        /// 오브젝트의 프로퍼티에 값을 설정
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public abstract bool SetPropertyValue(object source, string propertyName, object setValue);


        /// <summary>
        /// Get ILC Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public abstract CrossPlatform.Infrastructure.Interfaces.ICommonILC GetILC(System.Collections.IList source, Func<System.Threading.Tasks.Task> loadDataCallBack);

        /// <summary>
        /// LaunchUriAsync
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public abstract Task<bool> LaunchUriAsync(Uri uri);

        /// <summary>
        /// LaunchUriAsync
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options">LauncherOptions type</param>
        /// <returns></returns>
        public abstract Task<bool> LaunchUriAsync(Uri uri, object options);

        /// <summary>
        /// Toast Popup
        /// </summary>
        /// <param name="body"></param>
        /// <param name="header"></param>
        public abstract void ToastShow(string body, string header = null);

        /// <summary>
        /// 인터넷 연결상태 값 반환
        /// </summary>
        /// <returns></returns>
        public abstract bool GetAvaliableConnection();
    }
}
