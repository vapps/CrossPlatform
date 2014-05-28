using CrossPlatform.Infrastructure.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure
{
    public class ViewModelBase : BindableBase
    {
#if !W81_WP81
        protected Microsoft.Practices.Prism.PubSubEvents.IEventAggregator _eventAggregator;
#endif
        private bool _isBusy;
        /// <summary>
        /// 작업중
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); }
        }

        private string _title;
        /// <summary>
        /// 제목
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 실행 플랫폼 지정
        /// </summary>
        public Platforms Platform { get; set; }

        /// <summary>
        /// 뷰가 로드시에 실행되는 메소드
        /// </summary>
        protected virtual void Loaded() { }

        /// <summary>
        /// 뷰가 언로드시에 실행되는 메소드
        /// </summary>
        protected virtual void Unloaded() { }

        /// <summary>
        /// 사이즈 변경 이벤트 처리 메소드
        /// </summary>
        protected virtual void SizeChanged() { }

        private DelegateCommand _loadedCommand;
        /// <summary>
        /// 로디드 커맨드
        /// </summary>
        public DelegateCommand LoadedCommand
        {
            get { return _loadedCommand = _loadedCommand ?? new DelegateCommand(para => Loaded()); }
        }

        private DelegateCommand _unloadedCommand;
        /// <summary>
        /// 언로드 커맨드
        /// </summary>
        public DelegateCommand UnloadedCommand
        {
            get { return _unloadedCommand = _unloadedCommand ?? new DelegateCommand(para => Unloaded()); }
        }

        private DelegateCommand _sizeChangedCommand;
        /// <summary>
        /// 사이즈 체인지 커맨드
        /// </summary>
        public DelegateCommand SizeChangedCommand
        {
            get { return _sizeChangedCommand = _sizeChangedCommand ?? new DelegateCommand(para => SizeChanged()); }
        }

        private DelegateCommand _goBackCommand;
        /// <summary>
        /// 백 커맨드
        /// </summary>
        public DelegateCommand GoBackCommand
        {
            get 
            { 
                return _goBackCommand = _goBackCommand ?? new DelegateCommand(
                    para => FrameUtility.Instance.GoBack(),
                    para => FrameUtility.Instance.IsGoBack); }
        }
    }
}
