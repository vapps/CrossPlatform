using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class StorageUtility
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static StorageUtility Instance { get; set; }

        /// <summary>
        /// Save To Roaming
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract bool SaveToRoaming(string key, string value);

        /// <summary>
        /// Load From Roaming
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string LoadFromRoaming(string key);

        /// <summary>
        /// Clear TemporaryFolder
        /// </summary>
        /// <returns></returns>
        public abstract Task ClearTempFolderAsync();
    }
}
