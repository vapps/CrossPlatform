using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    // Summary:
    //     Represents the civic address data associated with a geographic location.
    public class CivicAddressPCL
    {
        // Summary:
        //     The name of the city.
        //
        // Returns:
        //     The name of the city.
        public string City { get; set; }
        //
        // Summary:
        //     The name of the country, represented by using a two-letter ISO-3166 country
        //     code.
        //
        // Returns:
        //     The name of the country, represented by using a two-letter ISO-3166 country
        //     code.
        public string Country { get; set; }
        //
        // Summary:
        //     The postal code of the current location.
        //
        // Returns:
        //     The postal code of the current location.
        public string PostalCode { get; set; }
        //
        // Summary:
        //     The name of the state or province.
        //
        // Returns:
        //     The name of the state or province.
        public string State { get; set; }
        //
        // Summary:
        //     The time at which the location data was obtained.
        //
        // Returns:
        //     The time at which the location data was obtained.
        public DateTimeOffset Timestamp { get; set; }
    }
}
