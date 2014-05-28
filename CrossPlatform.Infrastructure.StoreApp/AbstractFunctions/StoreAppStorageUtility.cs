using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using CrossPlatform.Infrastructure.Commons;
using CrossPlatform.Infrastructure.Models;
using CrossPlatform.Infrastructure.StoreApp.Commons;

// ReSharper disable once CheckNamespace
namespace CrossPlatform.Infrastructure.StoreApp
{
    /// <summary>
    ///     WriteableBitmapEx
    ///     https://writeablebitmapex.codeplex.com/
    /// </summary>
    public class StoreAppStorageUtility : StorageUtility
    {
        private const string COMPOSITE_SETTING = "CompositeSetting";

        public override bool SaveToRoaming(string key, string value)
        {
            ApplicationDataCompositeValue composite =
                ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] as ApplicationDataCompositeValue ??
                new ApplicationDataCompositeValue();
            if (composite.Any(p => p.Key == key))
            {
                composite.Remove(key);
            }
            composite.Add(key, value);

            ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] = composite;

            return true;
        }

        public override string LoadFromRoaming(string key)
        {
            object returnValue;
            var composite =
                ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] as ApplicationDataCompositeValue;
            if (composite == null)
            {
                return null;
            }
            composite.TryGetValue(key, out returnValue);

            return string.IsNullOrEmpty(returnValue as string) ? null : returnValue.ToString();
        }

        public override bool SaveToLocal(string key, string value)
        {
            ApplicationDataCompositeValue composite =
                ApplicationData.Current.LocalSettings.Values[COMPOSITE_SETTING] as ApplicationDataCompositeValue ??
                new ApplicationDataCompositeValue();
            if (composite.Any(p => p.Key == key))
            {
                composite.Remove(key);
            }
            composite.Add(key, value);

            ApplicationData.Current.LocalSettings.Values[COMPOSITE_SETTING] = composite;

            return true;
        }

        public override string LoadFromLocal(string key)
        {
            object returnValue;
            var composite =
                ApplicationData.Current.LocalSettings.Values[COMPOSITE_SETTING] as ApplicationDataCompositeValue;
            if (composite == null)
            {
                return null;
            }
            composite.TryGetValue(key, out returnValue);

            return string.IsNullOrEmpty(returnValue as string) ? null : returnValue.ToString();
        }

        /// <summary>
        ///     임시 폴더의 파일 삭제
        /// </summary>
        /// <returns></returns>
        public override async Task ClearTempFolderAsync()
        {
            IReadOnlyList<StorageFile> files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            if (files != null)
            {
                foreach (StorageFile item in files)
                {
                    await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

        /// <summary>
        /// Create folder
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="createFolderName"></param>
        /// <returns></returns>
        public override async Task<FolderInfoM> CreateFolderAsync(FolderInfoM parentFolder, string createFolderName)
        {
            var storageFolder = parentFolder.StorageFolder as StorageFolder;
            if (storageFolder == null) return null;
            try
            {
                var createFolder = await storageFolder.CreateFolderAsync(createFolderName);
                return new FolderInfoM
                {
                    StorageFolder = createFolder,
                    FolderName = createFolder.DisplayName,
                    Path = createFolder.Path,
                    Token = Cipher.encrypt(createFolder.Path),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     사진 폴더 내부에 폴더 검색, 존재하지 않으면 생성
        /// </summary>
        /// <returns></returns>
        public override async Task<object> GetFolderInPictureLibAsync(string folderName)
        {
            StorageFolder picFolder = KnownFolders.PicturesLibrary;
            if (picFolder == null) return null;

            IStorageItem workFolder = await picFolder.TryGetItemAsync(folderName) ??
                                      await picFolder.CreateFolderAsync(folderName);
            return workFolder;
        }

        /// <summary>
        ///     Folder Picker
        /// </summary>
        /// <param name="suggestedStartLocation"></param>
        /// <param name="fileTypeFilters"></param>
        /// <returns>FolderInfoM</returns>
        public override async Task<FolderInfoM> GetFolderAsync(CFPickerLocationId suggestedStartLocation,
            IList<string> fileTypeFilters)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation =
                    (PickerLocationId) Enum.Parse(typeof (PickerLocationId), ((int) suggestedStartLocation).ToString())
            };
            foreach (string item in fileTypeFilters)
            {
                folderPicker.FileTypeFilter.Add(item);
            }

            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder == null) return null;
            var folderInfo = new FolderInfoM
            {
                StorageFolder = folder,
                FolderName = folder.DisplayName,
                Path = folder.Path,
                Token = Cipher.encrypt(folder.Path),
            };

            StorageApplicationPermissions.FutureAccessList.AddOrReplace(folderInfo.Token, folder);
            return folderInfo;
        }

        public override async Task GetFolderAsync(FolderInfoM folderInfo)
        {
            var returnValue = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderInfo.Token);
            folderInfo.StorageFolder = returnValue;
        }

        public override async Task<IList<FolderInfoM>> GetSubFolderAsync(FolderInfoM folderInfo)
        {
            var folder = folderInfo.StorageFolder as StorageFolder;
            if (folder == null)
                return null;

            var subFolders = await folder.GetFoldersAsync();
            var returnFolders = (from kkk in subFolders
                select new FolderInfoM
                {
                    StorageFolder = kkk,
                    FolderName = kkk.DisplayName,
                    Path = kkk.Path,
                    Token = Cipher.encrypt(kkk.Path),
                }).ToList();
            return returnFolders;
        }

        /// <summary>
        ///     폴더에 있는 파일 목록 반환
        /// </summary>
        /// <returns></returns>
        public override async Task<IList<FileInfoM>> GetFilesAsync(FolderInfoM folderInfo)
        {
            if (folderInfo == null) return null;
            var workFolder = (StorageFolder) folderInfo.StorageFolder;
            var files = await workFolder.GetFilesAsync();
            if (files == null) return null;
            var returnFiles = (from kkk in files
                orderby kkk.DateCreated descending
                select new FileInfoM
                {
                    FileName = kkk.DisplayName,
                    ExtName = kkk.FileType,
                    StorageFile = kkk,
                    IsLocalFile = false,
                    Path = kkk.Path,
                    DateCreated = kkk.DateCreated.ToString(),
                }).ToList();

            return returnFiles;
        }

        public override async Task GetBitmapImageAsync(FileInfoM fileInfo)
        {
            if (fileInfo == null || fileInfo.StorageFile == null) return;

            var workFile = (StorageFile) fileInfo.StorageFile;

            var bitmapImage = new BitmapImage();

            using (IRandomAccessStream fileStream = await workFile.OpenAsync(FileAccessMode.Read))
            {
                // Set the image source to the selected bitmap
                ImageProperties property = await workFile.Properties.GetImagePropertiesAsync();
                bitmapImage.DecodePixelHeight = (int) property.Height;
                bitmapImage.DecodePixelWidth = (int) property.Width;

                await bitmapImage.SetSourceAsync(fileStream);
                fileInfo.Data = bitmapImage;
            }
        }

        public override async Task GetWriteableBitmapAsync(FileInfoM fileInfo, bool isThumbnail = true)
        {
            if (fileInfo == null || fileInfo.StorageFile == null) return;
            var workFile = (StorageFile) fileInfo.StorageFile;

            WriteableBitmap wb;
            if (isThumbnail)
            {
                //요약본 사이즈와 크기
                StorageItemThumbnail scaledImage =
                    await workFile.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem);
                if (scaledImage != null)
                {
                    wb = new WriteableBitmap((int) scaledImage.OriginalWidth, (int) scaledImage.OriginalHeight);
                    await wb.SetSourceAsync(scaledImage);
                    if (fileInfo.Data != null)
                        fileInfo.Data = null;
                    fileInfo.Data = wb;
                }
            }
            else
            {
                //원본 사이즈와 크기 - ImageProcessDiabloIII에서 호출하는 경우에는 ImageInfoM이라고 판단한다.
                ImageProperties property = await workFile.Properties.GetImagePropertiesAsync();
                wb = new WriteableBitmap((int) property.Width, (int) property.Height);
                using (IRandomAccessStream fileStream = await workFile.OpenAsync(FileAccessMode.Read))
                {
                    await wb.SetSourceAsync(fileStream);
                    var image = ((ImageInfoM) fileInfo);
                    if (image.WorkedImage != null) image.WorkedImage = null;
                    image.WorkedImage = wb;
                }
            }
        }

        /// <summary>
        ///     이미지 저장
        /// </summary>
        public override async Task SaveFileWriteableBitmapAsync(FolderInfoM saveFolder, FileInfoM saveFile, bool isShareTarget = false)
        {
            if (saveFolder == null || saveFolder.StorageFolder == null) return;
            var folder = (StorageFolder) saveFolder.StorageFolder;

            IStorageItem createFile = await folder.TryGetItemAsync(saveFile.FileName + saveFile.ExtName) ??
                                      await folder.CreateFileAsync(saveFile.FileName + saveFile.ExtName);

            using (IRandomAccessStream stream = await ((StorageFile) createFile).OpenAsync(FileAccessMode.ReadWrite))
            {
                var wb = (WriteableBitmap) ((ImageInfoM) saveFile).WorkedImage;
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                Stream pixelStream = wb.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint) wb.PixelWidth,
                    (uint) wb.PixelHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }
            saveFile.StorageFile = createFile;
            if(isShareTarget == false)
                saveFile.IsLocalFile = true;
        }

        public override string CopyFile(FileInfoM file)
        {
            string errorMessage = string.Empty;

            if (file.StorageFile == null || (file.StorageFile is StorageFile) == false)
            {
                errorMessage = "Have no StorageFile";
                return errorMessage;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetStorageItems(new List<StorageFile> {(StorageFile) file.StorageFile}, true);
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            try
            {
                Clipboard.SetContent(dataPackage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return errorMessage;
        }

        public override string CopyFiles(IList<FileInfoM> files)
        {
            string errorMessage = string.Empty;

            if (files.Any(p => p.StorageFile == null) || files.Any(p => (p.StorageFile is StorageFile) == false))
            {
                errorMessage = "Have no StorageFiles";
                return errorMessage;
            }

            var dataPackage = new DataPackage();
            var storageFiles = (from kkk in files
                select kkk.StorageFile).Cast<StorageFile>().ToList();
            dataPackage.SetStorageItems(storageFiles, true);
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            try
            {
                Clipboard.SetContent(dataPackage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return errorMessage;
        }

        public override async Task<string> DeleteFilesAsync(IList<FileInfoM> files)
        {
            var errorMessage = string.Empty;

            if (files.Any(p => p.StorageFile == null) || files.Any(p => (p.StorageFile is StorageFile) == false))
            {
                errorMessage = "Have no StorageFiles";
                return errorMessage;
            }

            try
            {
                foreach (var file in files)
                {
                    var storageFile = file.StorageFile as StorageFile;
                    if (storageFile != null)
                    {
                        await storageFile.DeleteAsync();
                        file.StorageFile = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return errorMessage;
        }

        public override async Task<string> DeleteFolderAsync(FolderInfoM folder)
        {
            var errorMessage = string.Empty;

            if (folder != null && folder.StorageFolder is StorageFolder)
            {
                try
                {
                    await ((StorageFolder)folder.StorageFolder).DeleteAsync();
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }

            return errorMessage;
        }

        public override async Task<string> MoveFilesAsync(IList<FileInfoM> sourceFiles, FolderInfoM destFolder)
        {
            var errorMessage = string.Empty;

            if (destFolder == null || destFolder.StorageFolder == null || !(destFolder.StorageFolder is StorageFolder))
            {
                errorMessage = "Folder information is not correct.";
                return errorMessage;
            }

            foreach (var file in sourceFiles)
            {
                var storageFile = file.StorageFile as StorageFile;
                if (storageFile != null)
                {
                    try
                    {
                        await storageFile.MoveAsync(destFolder.StorageFolder as StorageFolder);
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        break;
                    }
                }
            }

            return errorMessage;
        }

        /// <summary>
        ///     Get Part Image
        /// </summary>
        /// <param name="writeableBitmap"></param>
        /// <param name="sourceRect"></param>
        /// <returns></returns>
        public override object GetBitmapPart(object writeableBitmap, RectMini sourceRect)
        {
            WriteableBitmap wbm = BitmapFactory.New((int) sourceRect.Width, (int) sourceRect.Height);
            if (!(writeableBitmap is WriteableBitmap)) return wbm;
            var rect = new Rect(sourceRect.Left, sourceRect.Top, sourceRect.Width, sourceRect.Height);
            wbm.Blit(new Rect(0.0, 0.0, sourceRect.Width, sourceRect.Height), (WriteableBitmap) writeableBitmap, rect,
                WriteableBitmapExtensions.BlendMode.None);
            return wbm;
        }

        public override async Task RemoveFile(FileInfoM removeFile)
        {
            var storageFile = removeFile.StorageFile as IStorageFile;
            if (storageFile != null)
            {
                await storageFile.DeleteAsync();
            }
        }

        //public override async void OpenFolder(FolderInfoM folderInfo)
        //{
        //    var uri = new Uri("file:///" + folderInfo.Path);
        //    var result = await Windows.System.Launcher.LaunchUriAsync(uri);
        //    if (result == true)
        //    { 
        //    }
        //}
    }
}