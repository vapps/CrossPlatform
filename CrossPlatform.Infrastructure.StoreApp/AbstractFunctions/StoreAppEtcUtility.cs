using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using CrossPlatform.Infrastructure.Events;
using CrossPlatform.Infrastructure.Models;
using CrossPlatform.Infrastructure.StoreApp.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using NotificationsExtensions.ToastContent;

// ReSharper disable once CheckNamespace

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppEtcUtility : EtcUtility
    {
        //public override event EventHandler<NavigationArgs> Navigated;

        private static MessageDialog _msg;
        private static bool _isShow;
        private readonly IEventAggregator _eventAggregator;
        private Popup _currentPopup;
        private TaskCompletionSource<string> _taskCompletionSource;

        public StoreAppEtcUtility(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            //네트웍 연결 상태 변경 이벤트
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        /// <summary>
        ///     네트워크
        /// </summary>
        /// <param name="sender"></param>
        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            bool state = GetAvaliableConnection();
            StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                para =>
                    _eventAggregator.GetEvent<ActionEvent>()
                        .Publish(new KeyValuePair<string, object>("NetworkStatusChanged", state)), null);
        }

        /// <summary>
        ///     메시지박스
        /// </summary>
        /// <param name="content"></param>
        public override async void MsgBox(string content)
        {
            if (_msg == null)
            {
                _msg = new MessageDialog(content);
            }
            if (_isShow == false)
            {
                _isShow = true;
                await _msg.ShowAsync();
                _isShow = false;
            }
        }

        /// <summary>
        ///     메시지박스
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="ok"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public override async Task<bool> ConfirmAsync(string context, string title = "Confirm", string ok = "OK",
            string cancel = "Cancel")
        {
            var msg = new MessageDialog(context, title);
            msg.Commands.Add(new UICommand(ok, _ => { }, "0"));
            msg.Commands.Add(new UICommand(cancel, _ => { }, "1"));
            msg.DefaultCommandIndex = 1;
            IUICommand result = await msg.ShowAsync();
            return result.Id.ToString() == "0";
        }

        public override async Task<string> InputBoxTaskAsync(string message, string title = "InputBox", string ok = "OK",
            string cancel = "Cancel")
        {
            return await InputBoxAsync(message, title, ok, cancel);
        }

        private IAsyncOperation<string> InputBoxAsync(string message, string title = "InputBox", string ok = "OK",
            string cancel = "Cancel")
        {
            Rect bounds = Window.Current.CoreWindow.Bounds;
            _currentPopup = new Popup();

            var wide = new DefaultWideView(title, message, ok, cancel) {Width = bounds.Width, Height = bounds.Height};
            _currentPopup.Child = wide;
            _currentPopup.Closed +=
                (s, e) =>
                {
                    _taskCompletionSource.TrySetResult(_currentPopup.Tag.ToString());
                    if (_currentPopup != null) _currentPopup = null;
                };
            _currentPopup.IsOpen = true;
            return AsyncInfo.Run(WaitForClose);
        }

        private Task<string> WaitForClose(CancellationToken token)
        {
            _taskCompletionSource = new TaskCompletionSource<string>();
            token.Register(() =>
            {
                //CancelTokenRegister
                _currentPopup.IsOpen = false;
                _taskCompletionSource.SetCanceled();
                _currentPopup = null;
            });
            return _taskCompletionSource.Task;
        }


        /// <summary>
        ///     프로퍼티
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override T GetPropertyValue<T>(object source, string propertyName)
        {
            T returnValue = default(T);
            PropertyInfo pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            {
                object result = pinfo.GetValue(source);
                if (result != null)
                {
                    returnValue = (T) result;
                }
            }
            return returnValue;
        }

        /// <summary>
        ///     프로퍼티
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override object GetPropertyValue(object source, string propertyName)
        {
            object returnValue = null;

            PropertyInfo pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            {
                returnValue = pinfo.GetValue(source);
            }
            else
            {
                //Content 프로퍼티가 존재하는 오브젝트라면 그 내부에서 한번더 검색
                pinfo = source.GetType().GetRuntimeProperty("Content");
                if (pinfo == null) return null;
                object result = pinfo.GetValue(source);
                PropertyInfo cpinfo = result.GetType().GetRuntimeProperty(propertyName);
                if (cpinfo != null)
                {
                    returnValue = cpinfo.GetValue(source);
                }
            }
            return returnValue;
        }

        /// <summary>
        ///     프로퍼티
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override bool IsExistProperty(object source, string propertyName)
        {
            bool returnValue = false;

            PropertyInfo pinfo = source.GetType().GetRuntimeProperty(propertyName);
            if (pinfo != null)
            {
                returnValue = true;
            }
            else
            {
                //Content 프로퍼티가 존재하는 오브젝트라면 그 내부에서 한번더 검색
                pinfo = source.GetType().GetRuntimeProperty("Content");
                if (pinfo == null) return false;
                object result = pinfo.GetValue(source);
                PropertyInfo cpinfo = result.GetType().GetRuntimeProperty(propertyName);
                if (cpinfo != null)
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        /// <summary>
        ///     프로퍼티
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public override bool SetPropertyValue(object source, string propertyName, object setValue)
        {
            bool returnValue = false;

            try
            {
                PropertyInfo pinfo = source.GetType().GetRuntimeProperty(propertyName);
                if (pinfo != null)
                {
                    pinfo.SetValue(source, setValue);
                    returnValue = true;
                }
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            return returnValue;
        }

        /// <summary>
        ///     런처
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public override async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        ///     런처
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override async Task<bool> LaunchUriAsync(Uri uri, object options)
        {
            return await Launcher.LaunchUriAsync(uri, (LauncherOptions) options);
        }

        /// <summary>
        ///     토스트
        /// </summary>
        /// <param name="body"></param>
        /// <param name="header"></param>
        public override void ToastShow(string body, string header = null)
        {
            IToastNotificationContent templateContent;
            if (header == null)
            {
                templateContent = ToastContentFactory.CreateToastText01();
                ((IToastText01) templateContent).TextBodyWrap.Text = body;
            }
            else
            {
                templateContent = ToastContentFactory.CreateToastText02();
                ((IToastText02) templateContent).TextHeading.Text = header;
                ((IToastText02) templateContent).TextBodyWrap.Text = body;
            }

            ToastNotification toast = templateContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        ///     네트워크
        /// </summary>
        /// <returns></returns>
        public override bool GetAvaliableConnection()
        {
            ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (internetConnectionProfile == null)
            {
                return false;
            }
            NetworkConnectivityLevel ncl = internetConnectionProfile.GetNetworkConnectivityLevel();
            return ncl == NetworkConnectivityLevel.InternetAccess;
        }

        /// <summary>
        ///     팝업
        /// </summary>
        /// <param name="contentTypeName"></param>
        /// <param name="rect"></param>
        /// <param name="popupObject"></param>
        /// <returns></returns>
        public override object OpenPopup(string contentTypeName, RectMini rect, object popupObject = null)
        {
            Popup popup;

            var o = popupObject as Popup;
            if (o != null)
            {
                popup = o;
            }
            else
            {
                popup = new Popup();
                var contentType = Application.Current.GetType().GetTypeInfo().Assembly.GetType(contentTypeName);
                //Type contentType = Type.GetType(contentTypeName, false);
                if (contentType != null)
                {
                    object content = Activator.CreateInstance(contentType);
                    var child = content as FrameworkElement;
                    if (child != null)
                    {
                        popup.Child = child;
                    }
                }
            }
            popup.HorizontalOffset = rect.Left;
            popup.VerticalOffset = rect.Top;
            var element = popup.Child as FrameworkElement;
            if (element != null)
            {
                element.Width = rect.Width;
                element.Height = rect.Height;
            }

            //popup.IsLightDismissEnabled = true;
            popup.IsOpen = true;

            return popup;
        }
    }
}