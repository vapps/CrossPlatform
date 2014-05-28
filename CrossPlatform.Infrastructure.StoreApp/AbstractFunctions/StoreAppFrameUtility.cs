using CrossPlatform.Infrastructure.Events;
using CrossPlatform.Infrastructure.Models;
using CrossPlatform.Infrastructure.StoreApp.Commons;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml.Navigation;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppFrameUtility : FrameUtility
    {
        const string DEFAULT_KEY = "default_key";
        const string PAGE = "Page-";

        readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// 프라이머리 프레임
        /// </summary>
        internal Windows.UI.Xaml.Controls.Frame _frame;

        /// <summary>
        /// 세컨드리 프레임 - ShareTarget일때 새로운 앱이 실행된다. 그래서 프레임도 추가로 늘어난다
        /// </summary>
        static Windows.UI.Xaml.Controls.Frame _secondFrame;

        /// <summary>
        /// 세션 스테이트
        /// </summary>
        Dictionary<string, object> _frameSessionState = null;

        public StoreAppFrameUtility()
        { 
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="eventAggregator"></param>
        public StoreAppFrameUtility(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// 등록되어있는 프레임반환
        /// </summary>
        /// <returns></returns>
        public override object GetFrame()
        {
            return _frame;
        }

        /// <summary>
        /// 프레임 등록
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="sessionStateKey"></param>
        public override void RegisterFrame(object frame, string sessionStateKey = null)
        {
            if (_frame == null)
            {
                _frame = frame as Windows.UI.Xaml.Controls.Frame;
                _secondFrame = null;

                if (_frame == null) return;
                _frame.Navigated -= Frame_Navigated;
                _frame.Navigating -= Frame_Navigating;

                _frame.Navigated += Frame_Navigated;
                _frame.Navigating += Frame_Navigating;
                SuspensionManager.RegisterFrame(_frame, sessionStateKey ?? DEFAULT_KEY);
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
            if (frame == null) return;
            var naviArgs = new NavigationArgs
            {
                Content = frame.Content,
                NavigationMode =
                    (Models.NavigationMode) Enum.Parse(typeof (Models.NavigationMode), e.NavigationMode.ToString()),
                Parameter = e.SourcePageType,
                Uri = frame.BaseUri
            };

            if(StaticFunctions.BaseContext != null)
            {
                StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                    para => _eventAggregator.GetEvent<NavigatingEvent>().Publish(naviArgs), null);
            }
            else
            {
                _eventAggregator.GetEvent<NavigatingEvent>().Publish(naviArgs);
            }
        }

        /// <summary>
        /// Navigation To
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var naviArgs = new NavigationArgs
            {
                Content = e.Content,
                NavigationMode =
                    (Models.NavigationMode) Enum.Parse(typeof (Models.NavigationMode), e.NavigationMode.ToString()),
                Parameter = e.Parameter,
                Uri = e.Uri
            };

            //프레임의 세션 스테이트 값 복구
            var frameState = SuspensionManager.SessionStateForFrame(_frame);
            var pageKey = PAGE + _frame.BackStackDepth;
            if (naviArgs.NavigationMode == Models.NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = pageKey;
                var nextPageIndex = _frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = PAGE + nextPageIndex;
                }
                _frameSessionState = new Dictionary<string, object>();
                frameState[pageKey] = _frameSessionState;
            }
            else
            {
                _frameSessionState = (Dictionary<String, Object>)frameState[pageKey];
            }

            if (StaticFunctions.BaseContext != null)
            {
                StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                    para => _eventAggregator.GetEvent<NavigatedEvent>().Publish(naviArgs), null);
            }
            else
            {
                _eventAggregator.GetEvent<NavigatedEvent>().Publish(naviArgs);
            }
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
            if (_frame == null) return false;

            if (navigation == "GoBack")
            {
                if (_frame.CanGoBack) _frame.GoBack();
                return true;
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
            if (_frameSessionState == null) return false;
            _frameSessionState[key] = value;
            return true;
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

        public override void GoBack()
        {
            if(_frame.CanGoBack == true)
                _frame.GoBack();
        }

        public override bool IsGoBack
        {
            get { return _frame.CanGoBack; }
        }
    }
}
