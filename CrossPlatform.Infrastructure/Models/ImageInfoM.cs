using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace CrossPlatform.Infrastructure.Models
{
    /// <summary>
    ///     https://github.com/cureos/aforge
    ///     PCL version of AForge.NET Framework, https://code.google.com/p/aforge/
    ///     여기에서 소스 받아서 릴리즈로 빌드해서 사용함
    /// </summary>
    public class ImageInfoM : FileInfoM
    {
        private object _woredImage;

        /// <summary>
        ///     모델 생성자
        /// </summary>
        public ImageInfoM()
        {
            ItemImages = new ObservableCollection<ImageInfoM>();
        }

        /// <summary>
        /// 모델 생성자2
        /// </summary>
        /// <param name="fileInfo"></param>
        public ImageInfoM(FileInfoM fileInfo)
            : this()
        {
            FileName = fileInfo.FileName;
            ExtName = fileInfo.ExtName;
            StorageFile = fileInfo.StorageFile;
            IsLocalFile = fileInfo.IsLocalFile;
            Data = fileInfo.Data;
            Path = fileInfo.Path;
            DateCreated = fileInfo.DateCreated;
        }

        /// <summary>
        ///     작업한 이미지 WriteableBitmap
        /// </summary>
        [JsonIgnore]
        public object WorkedImage
        {
            get { return _woredImage; }
            set
            {
                _woredImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     아이템 이미지들
        /// </summary>
        [JsonIgnore]
        public IList<ImageInfoM> ItemImages { get; private set; }
    }
}