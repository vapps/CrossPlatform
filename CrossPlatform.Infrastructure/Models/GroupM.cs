using System.Collections;
using System.Collections.ObjectModel;
using CrossPlatform.Infrastructure.Interfaces;

namespace CrossPlatform.Infrastructure.Models
{
    /// <summary>
    ///     그룹 모델
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GroupM<T> : BindableBase, IVariableSizeItem where T : class
    {
        private IList _items;
        private IList _topItems;

        /// <summary>
        ///     생성자
        /// </summary>
        public GroupM()
        {
            TopItems = new ObservableCollection<T>();
            Items = new ObservableCollection<T>();
            IsMoreGroup = false;
        }

        /// <summary>
        ///     그룹코드
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        ///     그룹명
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///     설명
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     탑 아이템들
        /// </summary>
        public IList TopItems
        {
            get { return _topItems; }
            set { SetProperty(ref _topItems, value); }
        }

        /// <summary>
        ///     전체 아이템들
        /// </summary>
        public IList Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        /// <summary>
        ///     그룹 버튼을 눌러서 그룹 보기 화면으로 전화 가능 여부
        /// </summary>
        public bool IsMoreGroup { get; set; }

        public int Cols { get; set; }

        public int Rows { get; set; }
    }
}