using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class RectMini
    {
        // Summary:
        //     [SECURITY CRITICAL] Gets the y-axis value of the bottom of the rectangle.
        //
        // Returns:
        //     The y-axis value of the bottom of the rectangle. If the rectangle is empty,
        //     the value is System.Double.NegativeInfinity .
        public double Bottom { get; set; }
        //
        // Summary:
        //     [SECURITY CRITICAL] Gets or sets the height of the rectangle.
        //
        // Returns:
        //     A value that represents the height of the rectangle. The default is 0.
        public double Height { get; set; }
        //
        // Summary:
        //     [SECURITY CRITICAL] Gets the x-axis value of the left side of the rectangle.
        //
        // Returns:
        //     The x-axis value of the left side of the rectangle.
        public double Left { get; set; }
        //
        // Summary:
        //     [SECURITY CRITICAL] Gets the x-axis value of the right side of the rectangle.
        //
        // Returns:
        //     The x-axis value of the right side of the rectangle.
        public double Right { get; set; }
        //
        // Summary:
        //     [SECURITY CRITICAL] Gets the y-axis position of the top of the rectangle.
        //
        // Returns:
        //     The y-axis position of the top of the rectangle.
        public double Top { get; set; }
        //
        // Summary:
        //     [SECURITY CRITICAL] Gets or sets the width of the rectangle.
        //
        // Returns:
        //     A value that represents the width of the rectangle in pixels. The default
        //     is 0.
        public double Width { get; set; }
    }
}
