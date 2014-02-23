using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml.Navigation;
using CrossPlatform.Infrastructure.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using CrossPlatform.Infrastructure.StoreApp.Common;
using CrossPlatform.Infrastructure.Events;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppFrameUtility : FrameUtility
    {
        const string DEFAULT_KEY = "default_key";
        const string PAGE = "Page-";

        IEventAggregator _eventAggregator;

        /// <summary>
        /// 프레임
        /// </summary>
        static Windows.UI.Xaml.Controls.Frame _frame;
        /// <summary>
        /// 세션 스테이트
        /// </summary>
        Dictionary<string, object> _frameSessionState = null;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="eventAggregator"></param>
        public StoreAppFrameUtility(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// 프레임 등록
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="sessionStateKey"></param>
        public override void RegisterFrame(object frame, string sessionStateKey = null)
        {
            _frame = _frame ?? frame as Windows.UI.Xaml.Controls.Frame;
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
                _frame.Navigating -= Frame_Navigating;

                _frame.Navigated += Frame_Navigated;
                _frame.Navigating += Frame_Navigating;
            }
            if (sessionStateKey != null)
            {
                SuspensionManager.RegisterFrame(_frame, sessionStateKey);
            }
            else
            {
                SuspensionManager.RegisterFrame(_frame, DEFAULT_KEY);
            }
        }

        /// <summary>
        /// Navigation From
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var frame = sender as Windows.UI.Xaml.Controls.Frame;
            if (frame != null)
            {
                var naviArgs = new NavigationArgs();
                naviArgs.Content = frame.Content;
                naviArgs.NavigationMode = (CrossPlatform.Infrastructure.Models.NavigationMode)Enum.Parse(typeof(CrossPlatform.Infrastructure.Models.NavigationMode), e.NavigationMode.ToString());
                naviArgs.Parameter = e.SourcePageType;
                naviArgs.Uri = frame.BaseUri;

                StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                para =>
                {
                    _eventAggregator.GetEvent<NavigatingEvent>().Publish(naviArgs);
                }, null);
            }
        }

        /// <summary>
        /// Navigation To
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var naviArgs = new NavigationArgs();
            naviArgs.Content = e.Content;
            naviArgs.NavigationMode = (CrossPlatform.Infrastructure.Models.NavigationMode)Enum.Parse(typeof(CrossPlatform.Infrastructure.Models.NavigationMode), e.NavigationMode.ToString());
            naviArgs.Parameter = e.Parameter;
            naviArgs.Uri = e.Uri;

            //프레임의 세션 스테이트 값 복구
            var frameState = SuspensionManager.SessionStateForFrame(_frame);
            var _pageKey = PAGE + _frame.BackStackDepth;
            if (naviArgs.NavigationMode == Models.NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = _pageKey;
                int nextPageIndex = _frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = PAGE + nextPageIndex;
                }
                _frameSessionState = new Dictionary<string, object>();
                frameState[_pageKey] = _frameSessionState;
            }
            else
            {
                _frameSessionState = (Dictionary<String, Object>)frameState[_pageKey];
            }

            StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                para =>
                {
                    _eventAggregator.GetEvent<NavigatedEvent>().Publish(naviArgs);
                }, null);
        }

        /// <summary>
        /// 네비게이션
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="navigationParameter"></param>
        /// <returns></returns>
        public override bool Navigation(string navigation, object navigationParameter)
        {
            var returnValue = false;
            if (_frame == null) return returnValue;

            if (navigation == "GoBack")
            {
                if (_frame.CanGoBack == true) _frame.GoBack();
                return returnValue = true;
            }
            var t = Windows.UI.Xaml.Application.Current.GetType().GetTypeInfo().Assembly.GetType(navigation);
            if (t != null && t != typeof(Uri))
            {
                returnValue = _frame.Navigate(t, navigationParameter);
            }
            return returnValue;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool SetKeepData(string key, object value)
        {
            bool returnValue = false;
            if (_frameSessionState != null)
            {
                _frameSessionState[key] = value;
                returnValue = true;
            }
            return returnValue;
        }

        /// <summary>
        /// 데이터 조회
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object GetKeepData(string key)
        {
            object returnValue = null;
            if (_frameSessionState != null)
            {
                _frameSessionState.TryGetValue(key, out returnValue);
            }
            return returnValue;
        }
    }
}
