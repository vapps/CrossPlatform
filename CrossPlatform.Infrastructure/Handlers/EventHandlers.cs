using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Handlers
{
    public class IntEventArgs : EventArgs
    {
        public int IntArgs { get; set; }
    }

    public class UintEventArgs : EventArgs
    {
        public uint UintArgs { get; set; }
    }

    public class PositionChangeEventArgs : EventArgs
    {
        public CrossPlatform.Infrastructure.Models.GeocoordinatePCL PositionArgs { get; set; }
    }
}
