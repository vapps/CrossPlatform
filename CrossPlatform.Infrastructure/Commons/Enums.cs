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

}
