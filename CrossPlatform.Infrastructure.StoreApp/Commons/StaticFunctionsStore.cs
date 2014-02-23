using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    public class StaticFunctionsStore
    {
        private static StaticFunctionsStore _instance;
        /// <summary>
        /// 인스턴스
        /// </summary>
        public static StaticFunctionsStore Instance
        {
            get
            {
                return _instance = _instance ?? new StaticFunctionsStore();
            }
        }

        public StaticFunctionsStore()
        {
            UpdateFolder();
        }

        /// <summary>
        /// 비트맵 이미지 관리
        /// </summary>
        private ObservableCollection<KeyValuePair<string, object>> images = new ObservableCollection<KeyValuePair<string, object>>();

        /// <summary>
        /// 파일들
        /// </summary>
        private IReadOnlyList<StorageFile> files;

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="key"></param>
        /// <param name="image"></param>
        private void AddImage(string key, BitmapImage image)
        {
            if(string.IsNullOrEmpty(key)) return;

            if (images.Any(p => p.Key == key) == false)
            {
                var addItem = new KeyValuePair<string,object>(key, new WeakReference<object>(image));
                images.Add(addItem);
            }

        }

        /// <summary>
        /// 이미지 조회
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private BitmapImage GetImage(string key)
        {
            object returnValue = null;

            if (string.IsNullOrEmpty(key)) return null;

            var existItem = images.FirstOrDefault(p => p.Key == key);
            if (string.IsNullOrEmpty(existItem.Key) == false)
            {
                if (((WeakReference<object>)existItem.Value).TryGetTarget(out returnValue) == false)
                    images.Remove(existItem);
            }
            return returnValue as BitmapImage;
        }

        public void Init()
        {
            //인스 턴스를 생성을 하기위해
        }

        /// <summary>
        /// 폴더 업데이트
        /// </summary>
        private async void UpdateFolder()
        {
            files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
        }

        /// <summary>
        /// 문자열 이미지 uri를 받아서 이미지를 로컬에 저장하고 BitmapImage로 반환한다.
        /// </summary>
        /// <param name="imageUri"></param>
        /// <returns></returns>
        public async Task<BitmapImage> UriImageSaveLocalAsync(string imageUri, bool retry = true)
        {
            if (string.IsNullOrEmpty(imageUri) == true) return null;

            //폴더 초기화 될때까지 대기
            while (files == null)
            {
                await TaskEx.Delay(500);
            }

            //Stream
            var iuri = new Uri(imageUri, UriKind.Absolute);
            string filename = System.IO.Path.GetFileName(iuri.LocalPath);
            //메모리 내용확인
            var mbi = GetImage(filename);
            if (mbi != null)
            {
                return mbi;
            }

            Stream imageStream = null;      //기본 스트림
            //IRandomAccessStream
            InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
            //create bitmap
            BitmapImage bi = new BitmapImage();

            //폴더에 파일 존재 확인
            if (files.Any(p => p.Name == filename))
            {
                var localFile = files.First(p => p.Name == filename);
                bi.UriSource = new Uri(Path.Combine(ApplicationData.Current.TemporaryFolder.Path, localFile.Name));
                AddImage(filename, bi);
                //try
                //{
                //    imageStream = await localFile.OpenStreamForReadAsync();
                //}
                //catch (Exception)
                //{
                //    //파일 열때 에러가 발생하면, 파일이 존재하지 않기 때문일 수 있는..
                //    UpdateFolder();
                //    bi.UriSource = new Uri(imageUri);
                //    if (imageStream != null) imageStream.Dispose();
                //    if (ras != null) ras.Dispose();
                //    return bi;
                //}

                //await imageStream.CopyToAsync(ras.AsStreamForWrite());
                //if (ras.Size > 0)
                //{
                //    ras.Seek(0);
                //    await bi.SetSourceAsync(ras);
                //    //메모리에 저장
                //    AddImage(filename, bi);
                //}
                //else
                //{
                //    //파일 이상인듯
                //    await localFile.DeleteAsync();
                //    UpdateFolder();
                //    //재귀호출
                //    if (retry == false)
                //    {
                //        if (imageStream != null) imageStream.Dispose();
                //        if (ras != null) ras.Dispose();
                //        return await UriImageSaveLocalAsync(imageUri, true);
                //    }
                //    else
                //    {
                //        bi.UriSource = new Uri(imageUri);
                //        if (imageStream != null) imageStream.Dispose();
                //        if (ras != null) ras.Dispose();
                //        return bi;
                //    }
                //}
            }
            else
            {
                using (HttpClient hc = new HttpClient())
                {
                    try
                    {
                        imageStream = await hc.GetStreamAsync(imageUri);
                    }
                    catch (Exception ex)
                    {
                        //네트워크 상태가 끊어졌을 때
                        bi.UriSource = new Uri(imageUri);
                        if (imageStream != null) imageStream.Dispose();
                        if (ras != null) ras.Dispose();
                        return bi;
                    }
                    //Stream -> IRandomAccessStream
                    await imageStream.CopyToAsync(ras.AsStreamForWrite());
                    if (ras.Size > 0)
                    {
                        try
                        {
                            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ras);
                            BitmapFrame frame = await decoder.GetFrameAsync(0);
                            //파일로 저장
                            var bitmap = new WriteableBitmap((int)frame.PixelWidth, (int)frame.PixelHeight);
                            ras.Seek(0);
                            await bitmap.SetSourceAsync(ras);
                            var saveImage = await bitmap.SaveToFile(ApplicationData.Current.TemporaryFolder, filename, CreationCollisionOption.OpenIfExists);
                            UpdateFolder();
                            //UriSource로 이미지 넣어주고
                            bi.UriSource = new Uri(Path.Combine(ApplicationData.Current.TemporaryFolder.Path, saveImage.Name));

                            //이미지로 변환
                            //ras.Seek(0);
                            //await bi.SetSourceAsync(ras);
                            //메모리에 저장
                            AddImage(filename, bi);
                        }
                        catch (Exception)
                        {
                            //이미지가 너무 커서 저장할 수 없을 경우 그냥 이미지 uri를 넣어서 던짐
                            bi.UriSource = new Uri(imageUri);
                            if (imageStream != null) imageStream.Dispose();
                            if (ras != null) ras.Dispose();
                            return bi;
                        }
                    }
                    else
                    {
                        //재귀호출
                        if (retry == false)
                        {
                            if (imageStream != null) imageStream.Dispose();
                            if (ras != null) ras.Dispose();
                            return await UriImageSaveLocalAsync(imageUri, true);
                        }
                        else
                        {
                            bi.UriSource = new Uri(imageUri);
                            if (imageStream != null) imageStream.Dispose();
                            if (ras != null) ras.Dispose();
                            return bi;
                        }
                    }
                }
            }
            if(imageStream != null) imageStream.Dispose();
            if(ras != null) ras.Dispose();
            return bi;
        }

    }
}
