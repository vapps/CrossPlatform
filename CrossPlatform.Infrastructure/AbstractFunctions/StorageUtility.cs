using System.Collections.Generic;
using System.Threading.Tasks;
using CrossPlatform.Infrastructure.Commons;
using CrossPlatform.Infrastructure.Models;

// ReSharper disable once CheckNamespace
namespace CrossPlatform.Infrastructure
{
    public abstract class StorageUtility
    {
        /// <summary>
        ///     Instance
        /// </summary>
        public static StorageUtility Instance { get; set; }

        /// <summary>
        ///     Save To Roaming
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract bool SaveToRoaming(string key, string value);

        /// <summary>
        ///     Load From Roaming
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string LoadFromRoaming(string key);

        public abstract bool SaveToLocal(string key, string value);

        public abstract string LoadFromLocal(string key);

        /// <summary>
        ///     Clear TemporaryFolder
        /// </summary>
        /// <returns></returns>
        public abstract Task ClearTempFolderAsync();

        public abstract Task<FolderInfoM> CreateFolderAsync(FolderInfoM parentFolder, string createFolderName);

        public abstract Task<object> GetFolderInPictureLibAsync(string folderName);

        public abstract Task<FolderInfoM> GetFolderAsync(CFPickerLocationId suggestedStartLocation,
            IList<string> fileTypeFilters);

        /// <summary>
        /// FutureAccessList에서 폴더 접근 권한을 가지고 폴더 정보를 반환받아서 넣어 준다.
        /// </summary>
        /// <param name="folderInfo"></param>
        /// <returns></returns>
        public abstract Task GetFolderAsync(FolderInfoM folderInfo);

        /// <summary>
        /// Folder에서 SubFolder를 반환한다.
        /// </summary>
        /// <param name="folderInfo"></param>
        /// <returns></returns>
        public abstract Task<IList<FolderInfoM>> GetSubFolderAsync(FolderInfoM folderInfo);

        /// <summary>
        ///     GetFiles
        /// </summary>
        /// <returns></returns>
        public abstract Task<IList<FileInfoM>> GetFilesAsync(FolderInfoM folderInfo);

        public abstract Task GetBitmapImageAsync(FileInfoM file);

        /// <summary>
        ///     파일에서 WriteableBitmp 반환
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isThumbnail"></param>
        /// <returns></returns>
        public abstract Task GetWriteableBitmapAsync(FileInfoM file, bool isThumbnail = true);

        /// <summary>
        ///     WriteableBitmap Save
        /// </summary>
        /// <param name="saveFolder"></param>
        /// <param name="saveFile"></param>
        /// <param name="isShareTarget"></param>
        /// <returns></returns>
        public abstract Task SaveFileWriteableBitmapAsync(FolderInfoM saveFolder, FileInfoM saveFile,
            bool isShareTarget = false);

        /// <summary>
        ///     File Copy
        /// </summary>
        /// <param name="file"></param>
        public abstract string CopyFile(FileInfoM file);

        /// <summary>
        /// Files copy
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public abstract string CopyFiles(IList<FileInfoM> files);

        /// <summary>
        /// Delete files
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public abstract Task<string> DeleteFilesAsync(IList<FileInfoM> files);

        /// <summary>
        /// Delete Folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public abstract Task<string> DeleteFolderAsync(FolderInfoM folder);

        /// <summary>
        /// Move Folder
        /// </summary>
        /// <param name="sourceFiles"></param>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        public abstract Task<string> MoveFilesAsync(IList<FileInfoM> sourceFiles, FolderInfoM destFolder);

        /// <summary>
        ///     GetBitmapPart
        /// </summary>
        /// <param name="writeableBitmap">WriteableBitmap source</param>
        /// <param name="sourceRect">Rect</param>
        /// <returns>WriteableBitmap part</returns>
        public abstract object GetBitmapPart(object writeableBitmap, RectMini sourceRect);

        //public abstract void OpenFolder(FolderInfoM folderInfo);

        public abstract Task RemoveFile(FileInfoM removeFile);
    }
}