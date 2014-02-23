using CrossPlatform.Infrastructure.StoreApp.Commons;
using System;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Search;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppW8Utility : W8Utility
    {
        public override void SettingsPaneShow()
        {
            StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
            para =>
            {
                var view = Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView();
                if (view != null)
                {
                    Windows.UI.ApplicationSettings.SettingsPane.Show();
                }
            }, null);
        }

        public override string ApplicationViewValue()
        {
            string visualState = "FullScreenLandscape";
            var windowWidth = Window.Current.Bounds.Width;
            var windowHeight = Window.Current.Bounds.Height;

            if (windowWidth <= 500)
            {
                visualState = "Snapped_Detail";
            }
            else if (windowWidth <= 1366)
            {
                if (windowWidth < windowHeight)
                {
                    visualState = "FullScreenPortrait_Detail";
                }
                else
                {
                    visualState = "FilledOrNarrow";
                }
            }

            return visualState;
            //return Windows.UI.ViewManagement.ApplicationView.Value.ToString();
        }

        public override void SearchPaneShowOnKeyboardInput(bool enable)
        {
            //SearchPane.GetForCurrentView().ShowOnKeyboardInput = enable;
        }

        public override async void RegisterBackgroundTaskTimeTrigger(string taskName, string taskEntryPoint)
        {
            //원격을 이용한 작업 시 Background Task 등록을 하지 않는다(오류 방지용)
            if (Windows.System.RemoteDesktop.InteractiveSession.IsRemote) return;
            //이미 등록되어 있다면 바로 종료
            if (BackgroundTaskUtil.GetIsBackgroundTask(taskName) == true) return;

            //백그라운드 등록 서비스 알림 - 꼭 15분에 한번씩 실행할 필요는 없으니 막아 놓음
            await BackgroundExecutionManager.RequestAccessAsync();

            //백그라운드 서비스 등록해제
            BackgroundTaskUtil.UnregisterBackgroundTasks(taskName);
            //백그라운드 서비스 등록(15분 간격으로 발생, 인터넷이 연결되어있을때만 실행)
            var task = BackgroundTaskUtil.RegisterBackgroundTask(taskEntryPoint,
                                                      taskName,
                                                      new TimeTrigger(15, false),
                                                      new SystemCondition(SystemConditionType.InternetAvailable));
        }
    }
}
