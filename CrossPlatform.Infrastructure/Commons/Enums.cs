using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Commons
{
    public enum Platforms
    {
        None,
        Windows8,
        Windows81,
        WindowsPhone75,
        WindowsPhone8,
        Silverlight5
    }

    public enum Signs
    { 
        None,
        LiveId,
        FacebookId,
        TwitterId,
    }

    public enum CryptoCommand
    {
        ENCRYPT = 0x0001,
        DECRYPT = 0x0002,
    }

    public enum CFPickerLocationId
    {
        // Summary:
        //     The Documents library.
        DocumentsLibrary = 0,
        //
        // Summary:
        //     The Computer folder.
        ComputerFolder = 1,
        //
        // Summary:
        //     The Windows desktop.
        Desktop = 2,
        //
        // Summary:
        //     The Downloads folder.
        Downloads = 3,
        //
        // Summary:
        //     The HomeGroup.
        HomeGroup = 4,
        //
        // Summary:
        //     The Music library.
        MusicLibrary = 5,
        //
        // Summary:
        //     The Pictures library.
        PicturesLibrary = 6,
        //
        // Summary:
        //     The Videos library.
        VideosLibrary = 7,
    }

}
