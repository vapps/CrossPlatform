using System;
using CrossPlatform.Infrastructure.Models;

namespace CrossPlatform.Infrastructure
{
    public enum DistanceIn
    {
        Miles,
        Kilometers
    };

    public static class Haversine
    {
        /// <summary>
        ///     두지점 사이의 거리 반환
        ///     http://blogs.msdn.com/b/dragoman/archive/2010/09/29/wp7-code-distance-computations-with-the-geolocation-api.aspx
        /// </summary>
        /// <param name="in"></param>
        /// <param name="here"></param>
        /// <param name="there"></param>
        /// <returns></returns>
        public static double Between(this DistanceIn @in, GeocoordinatePCL here, GeocoordinatePCL there)
        {
            int r = (@in == DistanceIn.Miles) ? 3960 : 6371;
            double dLat = (there.Latitude - here.Latitude).ToRadian();
            var dLon = (there.Longitude - here.Longitude).ToRadian();
            double a = Math.Sin(dLat/2)*Math.Sin(dLat/2) +
                       Math.Cos(here.Latitude.ToRadian())*Math.Cos(there.Latitude.ToRadian())*
                       Math.Sin(dLon/2)*Math.Sin(dLon/2);
            double c = 2*Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = r*c;
            return d;
        }

        private static double ToRadian(this double val)
        {
            return (Math.PI/180)*val;
        }
    }
}