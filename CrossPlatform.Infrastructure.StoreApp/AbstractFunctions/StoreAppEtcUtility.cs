using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Reflection;
using CrossPlatform.Infrastructure.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using Windows.System;
using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Background;
using CrossPlatform.Infrastructure.StoreApp.Commons;
using Windows.ApplicationModel.Search;
using Windows.Networking.Connectivity;
using CrossPlatform.Infrastructure.Events;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppEtcUtility : EtcUtility
    {
        //public override event EventHandler<NavigationArgs> Navigated;
        IEventAggregator _eventAggregator;

        static MessageDialog _msg;
        static bool _isShow;

        public StoreAppEtcUtility(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            //네트웍 연결 상태 변경 이벤트
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var state = GetAvaliableConnection();
            StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
            para =>
            {
                _eventAggregator.GetEvent<ActionEvent>().Publish(new KeyValuePair<string, object>("NetworkStatusChanged", state));
            }, null);
        }

        public override async void MsgBox(string content)
        {
            if (_msg == null)
            {
                _msg = new MessageDialog(content);
            }
            if (_isShow == false)
            {
                _isShow = true;
                var result = await _msg.ShowAsync();
                _isShow = false;
            }
        }

        public override async Task<bool> ConfirmAsync(string context, string title = "Confirm")
        {
            MessageDialog msg = new MessageDialog(context, title);
            msg.Commands.Add(new UICommand("OK", _ => { }, "0"));
            msg.Commands.Add(new UICommand("Cancel", _ => { }, "1"));
            msg.DefaultCommandIndex = 1;
            var result = await msg.ShowAsync();
            if (result.Id.ToString() == "0")
            {
                return true;
            }
            return false;
        }

        public override T GetPropertyValue<T>(object source, string propertyName)
        {
            T returnValue = default(T);
            var pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            { 
                var result = pinfo.GetValue(source);
                if (result != null)
                {
                    returnValue = (T)result;
                }
            }
            return returnValue;
        }

        public override object GetPropertyValue(object source, string propertyName)
        {
            object returnValue = null;

            var pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            {
                returnValue = pinfo.GetValue(source);
            }
            else
            {
                //Content 프로퍼티가 존재하는 오브젝트라면 그 내부에서 한번더 검색
                pinfo = source.GetType().GetRuntimeProperty("Content");
                if (pinfo != null)
                {
                    var result = pinfo.GetValue(source);
                    var cpinfo = result.GetType().GetRuntimeProperty(propertyName);
                    if (cpinfo != null)
                    {
                        returnValue = cpinfo.GetValue(source);
                    }
                }
            }
            return returnValue;
        }

        public override bool IsExistProperty(object source, string propertyName)
        {
            bool returnValue = false;

            var pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            {
                returnValue = true;
            }
            else
            {
                //Content 프로퍼티가 존재하는 오브젝트라면 그 내부에서 한번더 검색
                pinfo = source.GetType().GetRuntimeProperty("Content");
                if (pinfo != null)
                {
                    var result = pinfo.GetValue(source);
                    var cpinfo = result.GetType().GetRuntimeProperty(propertyName);
                    if (cpinfo != null)
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }

        public override bool SetPropertyValue(object source, string propertyName, object setValue)
        {
            var returnValue = false;

            try
            {
                var pinfo = source.GetType().GetRuntimeProperty(propertyName);
                if (pinfo != null)
                {
                    pinfo.SetValue(source, setValue);
                    returnValue = true;
                }
            }
            catch (Exception)
            {
            }

            return returnValue;
        }

        public override Interfaces.ICommonILC GetILC(System.Collections.IList source, Func<Task> loadDataCallBack)
        {
            return new CrossPlatform.Infrastructure.StoreApp.Commons.CommonStoreILC(source, loadDataCallBack);
        }

        public override async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }

        public override async Task<bool> LaunchUriAsync(Uri uri, object options)
        {
            return await Launcher.LaunchUriAsync(uri, (LauncherOptions)options);
        }

        public override void ToastShow(string body, string header = null)
        {
            IToastNotificationContent templateContent = null;
            if (header == null)
            {
                templateContent = ToastContentFactory.CreateToastText01();
                ((IToastText01)templateContent).TextBodyWrap.Text = body;
            }
            else
            {
                templateContent = ToastContentFactory.CreateToastText02();
                ((IToastText02)templateContent).TextHeading.Text = header;
                ((IToastText02)templateContent).TextBodyWrap.Text = body;
            }

            ToastNotification toast = templateContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public override bool GetAvaliableConnection()
        {
            ConnectionProfile internetConnectionProfile = null;
            internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

//            try
//            {
//            }
//            catch (Exception)
//            {
//#if DEBUG
//                throw;
//#endif
//            }

            if (internetConnectionProfile == null)
            {
                return false;
            }
            NetworkConnectivityLevel ncl = internetConnectionProfile.GetNetworkConnectivityLevel();
            if (ncl == NetworkConnectivityLevel.InternetAccess)
                return true;

            return false;
        }
    }

}
