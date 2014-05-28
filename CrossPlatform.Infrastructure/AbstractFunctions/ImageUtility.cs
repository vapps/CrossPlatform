using System.Threading.Tasks;
using CrossPlatform.Infrastructure.Models;

namespace CrossPlatform.Infrastructure
{
    public abstract class ImageUtility
    {
        /// <summary>
        ///     Instance
        /// </summary>
        public static ImageUtility Instance { get; set; }

        /// <summary>
        ///     DiabloIII Image processing
        /// </summary>
        /// <param name="workImageInfo"></param>
        /// <param name="isSizeCheck"></param>
        /// <returns></returns>
        public abstract Task ImageProcessDiabloIIIAsync(ImageInfoM workImageInfo, bool isSizeCheck = true);

        /// <summary>
        ///     DiabloIII Image processing from ShareTarget
        /// </summary>
        /// <param name="workImageInfo"></param>
        /// <returns></returns>
        public abstract Task ImageProcessDiabloIIIFromShareAsync(ImageInfoM workImageInfo);
    }
}