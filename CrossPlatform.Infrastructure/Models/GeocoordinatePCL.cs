using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class GeocoordinatePCL
    {
        // Summary:
        //     The accuracy of the location in meters.
        //
        // Returns:
        //     The accuracy in meters.
        public double Accuracy { get; set; }
        //
        // Summary:
        //     The altitude of the location, in meters.
        //
        // Returns:
        //     The altitude in meters.
        public double? Altitude { get; set; }
        //
        // Summary:
        //     The accuracy of the altitude, in meters.
        //
        // Returns:
        //     The accuracy of the altitude.
        public double? AltitudeAccuracy { get; set; }
        //
        // Summary:
        //     The current heading in degrees relative to true north.
        //
        // Returns:
        //     The current heading in degrees relative to true north.
        public double? Heading { get; set; }
        //
        // Summary:
        //     The latitude in degrees.
        //
        // Returns:
        //     The latitude in degrees.
        public double Latitude { get; set; }
        //
        // Summary:
        //     The longitude in degrees.
        //
        // Returns:
        //     The longitude in degrees.
        public double Longitude { get; set; }
        //
        // Summary:
        //     The speed in meters per second.
        //
        // Returns:
        //     The speed in meters per second.
        public double? Speed { get; set; }
        //
        // Summary:
        //     The time at which the location was determined.
        //
        // Returns:
        //     The time at which the location was determined.
        public DateTimeOffset Timestamp { get; set; }
    }
}
