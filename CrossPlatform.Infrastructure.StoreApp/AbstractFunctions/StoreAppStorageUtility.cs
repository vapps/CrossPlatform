using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppStorageUtility : StorageUtility
    {
        const string COMPOSITE_SETTING = "CompositeSetting";

        public override bool SaveToRoaming(string key, string value)
        {
            var returnValue = false;

            var composite = Windows.Storage.ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] as Windows.Storage.ApplicationDataCompositeValue;
            if (composite == null)
            {
                composite = new ApplicationDataCompositeValue();
            }
            if (composite.Any(p => p.Key == key))
            {
                composite.Remove(key);
            }
            composite.Add(key, value);

            ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] = composite;
            returnValue = true;

            return returnValue;
        }

        public override string LoadFromRoaming(string key)
        {
            object returnValue = null;
            var composite = ApplicationData.Current.RoamingSettings.Values[COMPOSITE_SETTING] as Windows.Storage.ApplicationDataCompositeValue;
            if (composite == null)
            {
                return null;
            }
            composite.TryGetValue(key, out returnValue);

            return string.IsNullOrEmpty(returnValue as string) ? null : returnValue.ToString();
        }

        /// <summary>
        /// 임시 폴더의 파일 삭제
        /// </summary>
        /// <returns></returns>
        public override async Task ClearTempFolderAsync()
        {
            var files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            if (files != null)
            {
                foreach (var item in files)
                {
                    await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

    }
}
