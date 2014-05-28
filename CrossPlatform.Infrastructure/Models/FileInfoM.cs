using Newtonsoft.Json;

namespace CrossPlatform.Infrastructure.Models
{
    public class FileInfoM : BindableBase
    {
        private object _data;
        private string _extName;
        private string _fileName;
        private bool _isLocalFile;

        /// <summary>
        ///     화면 표시용 이름
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     전체 경로
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     확장자명
        /// </summary>
        public string ExtName
        {
            get { return _extName; }
            set
            {
                _extName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     StorageFolder
        /// </summary>
        [JsonIgnore]
        public object StorageFile { get; set; }

        /// <summary>
        ///     로컬 파일 여부, 메모리에 있으면 false, 스토리지에 있으면 true, 이미지인 경우 true이면 작업완료
        /// </summary>
        public bool IsLocalFile
        {
            get { return _isLocalFile; }
            set
            {
                _isLocalFile = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     이미지 파일이면 이미지가 아니면 다른 오브젝트가
        /// </summary>
        [JsonIgnore]
        public object Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     생성일시
        /// </summary>
        [JsonIgnore]
        public string DateCreated { get; set; }

        public FileInfoM ShallowCopy()
        {
            var returnValue = (FileInfoM) MemberwiseClone();
            return returnValue;
        }
    }
}