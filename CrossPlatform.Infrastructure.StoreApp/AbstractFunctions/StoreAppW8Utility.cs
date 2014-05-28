using CrossPlatform.Infrastructure.Events;
using CrossPlatform.Infrastructure.Models;
using CrossPlatform.Infrastructure.StoreApp.Commons;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppW8Utility : W8Utility
    {
        readonly IEventAggregator _eventAggregator;

        private DataTransferManager _dataTransferManager;
        private DataRequest _dataRequest;

        public StoreAppW8Utility(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

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
            var visualState = "FullScreenLandscape";
            var windowWidth = Window.Current.Bounds.Width;
            var windowHeight = Window.Current.Bounds.Height;

            if (windowWidth <= 500)
            {
                visualState = "Snapped_Detail";
            }
            else if (windowWidth <= 1366)
            {
                visualState = windowWidth < windowHeight ? "FullScreenPortrait_Detail" : "FilledOrNarrow";
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

        public override void SetDataRequest()
        {
            //DataRequest 처리 시작
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
            //DataRequest에 등록할 데이터를 수신할 이벤트어그리게이트 등록
            _eventAggregator.GetEvent<ActionEvent>().Subscribe(ActionEventProcess, ThreadOption.PublisherThread, false, args => string.IsNullOrEmpty(args.Key) == false);
        }

        /// <summary>
        /// DataRequest 이벤트 수신
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            if (_dataRequest != null) _dataRequest = null;
            _dataRequest = e.Request;
            _eventAggregator.GetEvent<ActionEvent>().Publish(new KeyValuePair<string, object>("DataRequested", ""));
        }

        /// <summary>
        /// DataRequest에 등록할 데이터 수신
        /// </summary>
        /// <param name="args"></param>
        public void ActionEventProcess(KeyValuePair<string, object> args)
        {
            switch (args.Key)
            {
                case "DataRequested_SelectedItemImages":
                    var requestData = _dataRequest.Data;
                    var data = (DataRequestM)args.Value;
                    requestData.Properties.Title = data.Title;
                    requestData.Properties.Description = data.Description;

                    var items = data.ListIStorageItem as IList<object>;
                    if(items != null)
                    {
                        requestData.SetStorageItems(items.Cast<IStorageFile>());
                    }

                    var itemImage = data.SingleIStorageImageFile as IStorageFile;
                    if(itemImage != null)
                    {
                        var imageStreamRef = RandomAccessStreamReference.CreateFromFile(itemImage);
                        requestData.Properties.Thumbnail = imageStreamRef;
                        requestData.SetBitmap(imageStreamRef);
                    }
                    break;
            }
        }

        public override void UnsetDataRequest()
        {
            _dataTransferManager.DataRequested -= OnDataRequested;
            _eventAggregator.GetEvent<ActionEvent>().Unsubscribe(ActionEventProcess);
        }

        public override void ShowShareUI()
        {
            DataTransferManager.ShowShareUI();
        }
    }
}
