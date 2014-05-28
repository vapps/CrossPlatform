using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CrossPlatform.Infrastructure.StoreApp.Behaviors
{
    public class GridViewSelectedItemsBehavior : DependencyObject, Microsoft.Xaml.Interactivity.IBehavior
    {
        /// <summary>
        /// 뷰 모델의 프로퍼티 - 반드시 object로 만든다. 다른형은 프로퍼티 추가가 않된다.
        /// </summary>
        public object SelectedItems
        {
            get { return GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// 비헤이비어에 붙어있는 프로퍼티
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(object), typeof(GridViewSelectedItemsBehavior), new PropertyMetadata(null, OnSelectedItemsChanged));


        /// <summary>
        /// 비헤이비어 프로퍼티 체인지 이벤트
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (GridViewSelectedItemsBehavior)d;
            //이전 프로퍼티에 붙어있던 이벤트 제거
            var oldCollection = e.OldValue as INotifyCollectionChanged;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= source.ViewModelToControl_CollectionChanged;
            }
            //새 프로퍼티에 이벤트 연결
            var newCollection = e.NewValue as INotifyCollectionChanged;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += source.ViewModelToControl_CollectionChanged;
            }
        }

        /// <summary>
        /// 연결되어있는 오브젝트(여기서는 GridView)
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }

        /// <summary>
        /// GridView에 연결 될때 실행되는 메소드
        /// </summary>
        /// <param name="associatedObject"></param>
        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !DesignMode.DesignModeEnabled)
            {
                if (AssociatedObject != null)
                    throw new InvalidOperationException("Cannot attach behavior multiple times.");

                AssociatedObject = associatedObject;
                var control = AssociatedObject as GridView;
                if (control == null)
                {
                    throw new InvalidOperationException("Cannot attach behavior you must have GridView.");
                }
                //SelectedItems가 Runtime Object라서 IObservableVector<object>로 변환해서 VectorChanged를 사용
                ((IObservableVector<object>)control.SelectedItems).VectorChanged += ControlToViewModel_VectorChanged;
            }

        }

        /// <summary>
        /// GridView 연결 해제될때 실행되는 메소드
        /// </summary>
        public void Detach()
        {
            if (SelectedItems != null)
            {
                ((INotifyCollectionChanged)SelectedItems).CollectionChanged -= ViewModelToControl_CollectionChanged;
            }

            if (AssociatedObject != null)
            {
                ((IObservableVector<object>)((GridView)AssociatedObject).SelectedItems).VectorChanged -= ControlToViewModel_VectorChanged;
            }
            AssociatedObject = null;
        }

        /// <summary>
        /// 이벤트 중복 방지
        /// </summary>
        private bool _isLock;

        /// <summary>
        /// 컨트롤에서 발생한 이벤트를 이용해서 뷰모델의 프로퍼티에 작업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="event"></param>
        void ControlToViewModel_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            if (_isLock == true) return;
            _isLock = true;
            //Reset, ItemInserted만 발생함
            switch (@event.CollectionChange)
            {
                case CollectionChange.Reset:
                    ((IList)SelectedItems).Clear();
                    break;
                case CollectionChange.ItemInserted:
                    var item = sender.ElementAt((int)@event.Index);
                    ((IList)SelectedItems).Add(item);
                    break;
                case CollectionChange.ItemRemoved:
                    if (true) { }     //디버그
                    break;
                case CollectionChange.ItemChanged:
                    if (true) { }     //디버그
                    break;
            }
            _isLock = false;
        }

        /// <summary>
        /// 뷰모델의 프로퍼티에서 발생한 이벤트를 이용해서 컨트롤에 작업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewModelToControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isLock == true) return;
            _isLock = true;

            var control = AssociatedObject as GridView;

            //Reset처리
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (control != null) control.SelectedItems.Clear();
            }

            //삭제된 아이템 처리
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (control != null && control.SelectedItems.Contains(item) == true)
                    {
                        control.SelectedItems.Remove(item);
                    }
                }
            }
            //추가된 아이템 처리
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (control != null && control.SelectedItems.Contains(item) == false)
                    {
                        control.SelectedItems.Add(item);
                    }
                }
            }
            _isLock = false;
        }


    }

}
