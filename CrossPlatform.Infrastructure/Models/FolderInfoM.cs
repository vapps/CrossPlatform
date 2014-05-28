using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace CrossPlatform.Infrastructure.Models
{
    public class FolderInfoM : BindableBase
    {
        private string _folderName;
        /// <summary>
        /// 화면 표시용 이름
        /// </summary>
        public string FolderName 
        {
            get { return _folderName; }
            set { _folderName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 폴더 전체 경로
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// GUID 사용
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// StorageFolder 
        /// </summary>
        [JsonIgnore]
        public object StorageFolder { get; set; }

        private IList<FileInfoM> _files;
        /// <summary>
        /// 폴더내 이미지 목록
        /// </summary>
        [JsonIgnore]
        public IList<FileInfoM> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged(); }
        }

        private FileInfoM _firstFile;
        /// <summary>
        /// 대표 이미지(파일)
        /// </summary>
        [JsonIgnore]
        public FileInfoM FirstFile
        {
            get { return _firstFile; }
            set { _firstFile = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 폴더 작업 확인 여부
        /// </summary>
        [JsonIgnore]
        public bool IsWorked { get; set; }

        public FolderInfoM()
        {
            Files = new ObservableCollection<FileInfoM>();
        }

        public FolderInfoM ShallowCopy()
        {
            FolderInfoM returnValue = (FolderInfoM)this.MemberwiseClone();
            returnValue.Files = new ObservableCollection<FileInfoM>();
            return returnValue;
        }
    }
}
