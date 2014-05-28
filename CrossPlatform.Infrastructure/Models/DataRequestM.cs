using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class DataRequestM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 리스트 IStorageItem
        /// </summary>
        public object ListIStorageItem { get; set; }
        /// <summary>
        /// 단품 IStorageFile - 이미지
        /// </summary>
        public object SingleIStorageImageFile { get; set; }

    }
}
