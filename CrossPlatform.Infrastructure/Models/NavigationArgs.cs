using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    // Summary:
    //     Specifies the navigation stack characteristics of a navigation.
    public enum NavigationMode
    {
        // Summary:
        //     Navigation is to a new instance of a page (not going forward or backward
        //     in the stack).
        New = 0,
        //
        // Summary:
        //     Navigation is going backward in the stack.
        Back = 1,
        //
        // Summary:
        //     Navigation is going forward in the stack.
        Forward = 2,
        //
        // Summary:
        //     Navigation is to the current page (perhaps with different data).
        Refresh = 3,
    }

    public class NavigationArgs : EventArgs
    {
        public object Content { get; set; }
        public NavigationMode NavigationMode { get; set; }
        public object Parameter { get; set; }
        public Uri Uri { get; set; }
    }
}
