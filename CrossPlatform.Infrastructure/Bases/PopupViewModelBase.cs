using CrossPlatform.Infrastructure.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure
{
    public class PopupViewModelBase : BindableBase
    {
        //private WeakReference _parentPopup;
        ///// <summary>
        ///// Popup 뷰모델의 부모 : Popup object
        ///// </summary>
        //public object ParentPopup 
        //{
        //    get 
        //    {
        //        return _parentPopup != null && _parentPopup.IsAlive ? _parentPopup.Target : null;
        //    }
        //    set 
        //    {
        //        if (_parentPopup != null && _parentPopup.IsAlive == true && _parentPopup.Target.Equals(value) == false)
        //        {
        //            _parentPopup = null;
        //        }
        //        _parentPopup = new WeakReference(value);
        //        OnPropertyChanged();
        //    } 
        //}

        public object ParentPopup { get; set; }

        private bool _isBusy;
        /// <summary>
        /// 작업중
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
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

    }
}
