using System;

namespace CrossPlatform.Infrastructure.Models
{
    /// <summary>
    /// Windows.ApplicationModel.DataTransfer.DataPackagePropertySetView
    /// </summary>
    public class ShareOperationDataM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PackageFamilyName { get; set; }
        public Uri ContentSourceWebLink { get; set; }
        public Uri ContentSourceApplicationLink { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public object LogoBackgroundColor { get; set; }
        /// <summary>
        /// IRandomAccessStreamReference
        /// </summary>
        public object Square30x30Logo { get; set; }
        /// <summary>
        /// RandomAccessStreamReference
        /// </summary>
        public object Thumbnail { get; set; }
        public string QuickLinkId { get; set; }

        /// <summary>
        /// IRandomAccessStreamReference
        /// </summary>
        public object SharedBitmapStreamRef { get; set; }
        /// <summary>
        /// BitmapImage
        /// </summary>
        public object SharedBitmapImage { get; set; }
        /// <summary>
        /// Bitmap or WriteableBitmap
        /// </summary>
        public object SharedBitmap { get; set; }

        /// <summary>
        /// IReadOnlyList<IStorageItem>
        /// </summary>
        public object SharedStorageItems { get; set; }
    }
}