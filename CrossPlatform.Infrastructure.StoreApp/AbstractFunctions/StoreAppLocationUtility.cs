using CrossPlatform.Infrastructure.Handlers;
using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppLocationUtility : CrossPlatform.Infrastructure.LocationUtitlity
    {
        Geolocator _geolocator;
        Geoposition _geoposition;

        GeocoordinatePCL _defaultGeoPosition;
        CivicAddressPCL _defaultCivicAddress;

        /// <summary>
        /// 기본 위치 값을 반환
        /// </summary>
        /// <returns></returns>
        public override GeocoordinatePCL GetDefaultGeocoordinate()
        {
            return _defaultGeoPosition;
        }

        /// <summary>
        /// 위치 반환 - 비동기로 만들면 스레드 사라짐
        /// </summary>
        /// <returns></returns>
        public override GeocoordinatePCL GetGeocoordinate()
        {
            GeocoordinatePCL returnValue = null;
            if (_geoposition == null)
            {
                returnValue = _defaultGeoPosition;
            }
            else
            {
                returnValue = new GeocoordinatePCL
                {
                    Accuracy = _geoposition.Coordinate.Accuracy,
                    Altitude = _geoposition.Coordinate.Point.Position.Altitude,
                    AltitudeAccuracy = _geoposition.Coordinate.AltitudeAccuracy,
                    Heading = _geoposition.Coordinate.Heading,
                    Latitude = _geoposition.Coordinate.Point.Position.Latitude,
                    Longitude = _geoposition.Coordinate.Point.Position.Longitude,
                    Speed = _geoposition.Coordinate.Speed,
                    Timestamp = _geoposition.Coordinate.Timestamp
                };
            }
            return returnValue;
        }

        /// <summary>
        /// 위치의 주소 반환
        /// </summary>
        /// <returns></returns>
        public override CivicAddressPCL GetCivicAddress()
        {
            CivicAddressPCL returnValue = null;
            if (_geoposition == null)
            {
                returnValue = _defaultCivicAddress;
            }
            else
            {
                returnValue = new CivicAddressPCL
                {
                    City = _geoposition.CivicAddress.City,
                    Country = _geoposition.CivicAddress.Country,
                    PostalCode = _geoposition.CivicAddress.PostalCode,
                    State = _geoposition.CivicAddress.State,
                    Timestamp = _geoposition.CivicAddress.Timestamp
                };
            }
            return returnValue;
        }

        public override void Init()
        {
            _geolocator = new Geolocator();
            //_geolocator.MovementThreshold = 1;
            //_geolocator.ReportInterval = 1;
            _geolocator.PositionChanged += geolocator_PositionChanged;

            _defaultGeoPosition = new GeocoordinatePCL
                {
                    Accuracy = 1.0,
                    Altitude = 13,
                    Latitude = 37.566535,
                    Longitude = 126.977969,
                    Timestamp = DateTime.Now
                };
            _defaultCivicAddress = new CivicAddressPCL 
                {
                    City = "Seoul",
                    Country = "ko-KR",
                    PostalCode = "",
                    State = "",
                    Timestamp = DateTimeOffset.Now
                };
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs para)
        {
            //System.Diagnostics.Debug.WriteLine("geolocator_PositionChanged " + para.Position.Coordinate.Point.Position.Latitude.ToString() + " " + para.Position.Coordinate.Point.Position.Longitude.ToString());
            _geoposition = para.Position;
            if (_geoposition != null)
            {
                _geolocator.MovementThreshold = 10;
                _geolocator.ReportInterval = 1000;
            }
        }


    }
}
