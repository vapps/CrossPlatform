using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    public class CommonStoreILC : BindableBase, ISupportIncrementalLoading, INotifyCollectionChanged, IList, IDisposable, CrossPlatform.Infrastructure.Interfaces.ICommonILC
    {
        public CommonStoreILC(IList source, Func<Task> loadDataCallback)
        {
            Items = source;
            _loadDataCallback = loadDataCallback;
            HasMoreItems = true;
        }

        public CommonStoreILC()
        { 
        }

        /// <summary>
        /// Max Count
        /// </summary>
        public int MaxCount { get; set; }

        private bool _isBusy;
        /// <summary>
        /// 사용중 여부
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private IList _items;
        /// <summary>
        /// Collection
        /// </summary>
        public IList Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private Func<Task> _loadDataCallback;

        #region ISupportIncrementalLoading

        private bool _hasMoreItems;
        /// <summary>
        /// 컨트롤에서 이 프로퍼티를 사용해서 조회 사용여부 확인
        /// </summary>            
        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
            set { SetProperty(ref _hasMoreItems, value); }
        }

        public Windows.Foundation.IAsyncOperation<Windows.UI.Xaml.Data.LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return System.Runtime.InteropServices.WindowsRuntime.AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        async Task<Windows.UI.Xaml.Data.LoadMoreItemsResult> LoadMoreItemsAsync(System.Threading.CancellationToken c, uint count)
        {
            if (Items == null || IsBusy == true)
            {
                return new Windows.UI.Xaml.Data.LoadMoreItemsResult() { Count = 0 };
            }

            IsBusy = true;
            //더이상 호출이 되지 않도록 막음
            HasMoreItems = false;

            int start = Items.Count;
            await _loadDataCallback.Invoke();
            int end = Items.Count;
            int addCount = end - start;

            var addList = Items.Cast<object>().Skip(start).Take(addCount);
            if (CollectionChanged != null)
            {
                var para = new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, addList, null);
                CollectionChanged(this, para);
            }
            HasMoreItems = MaxCount > Items.Count;
            IsBusy = false;

            return new Windows.UI.Xaml.Data.LoadMoreItemsResult() { Count = (uint)addCount };
        }
        #endregion

        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            Items.Clear();
        }

        bool IList.Contains(object value)
        {
            return Items == null ? false : Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return Items == null ? -1 : Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            Items.Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        void IList.Remove(object value)
        {
            Items.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return Items == null ? null : Items[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items == null ? null : Items.GetEnumerator();
        }

        public void Dispose()
        {
            Items.Clear();
            Items = null;
            _loadDataCallback = null;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (Items == null)
            {
                return;
            }
            Items.CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return Items == null ? 0 : Items.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return Items.SyncRoot; }
        }


    }

}
