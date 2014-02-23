using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class CommonILC<T> : ObservableCollection<T>, Windows.UI.Xaml.Data.ISupportIncrementalLoading
    {
        public event EventHandler<CrossPlatform.Infrastructure.Handlers.UintEventArgs> LoadMoreItemsEvent;

        private uint currentPage = 0;
        /// <summary>
        /// 현재 페이지
        /// </summary>
        public uint CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        private bool isBusy = false;
        /// <summary>
        /// 사용중 여부
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        private bool _hasMoreItems = true;
        /// <summary>
        /// 컨트롤에서 이 프로퍼티를 사용해서 조회 사용여부 확인
        /// </summary>            
        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
            set
            {
                _hasMoreItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HasMoreItems"));
            }
        }


        /// <summary>
        /// 생성자
        /// </summary>
        public CommonILC()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                HasMoreItems = false;
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="source">컬렉션</param>
        public CommonILC(System.Collections.ICollection source)
        {
            foreach (var item in source.Cast<T>())
	        {
                this.Add(item);
	        }
        }

        public ICollection<T> ILCItems 
        {
            get { return base.Items; }
        }

        /// <summary>
        /// 데이터 추가 조회 메소드 - 컨트롤에서 발생됨
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (IsBusy)
            {
                return null;
            }
            HasMoreItems = false;
            IsBusy = true;

            return Task.Run<LoadMoreItemsResult>(
                () =>
                {
                    if (LoadMoreItemsEvent != null)
                    {
                        var para = new CrossPlatform.Infrastructure.Handlers.UintEventArgs() { UintArgs = CurrentPage };
                        LoadMoreItemsEvent(this, para);
                    }
                    return new LoadMoreItemsResult()
                    {
                        Count = count
                    };
                }).AsAsyncOperation<LoadMoreItemsResult>();
        }

    }
}
