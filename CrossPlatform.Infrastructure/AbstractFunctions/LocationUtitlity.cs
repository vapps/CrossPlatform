using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class LocationUtitlity
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static LocationUtitlity Instance { get; set; }

        /// <summary>
        /// Get Geocoordinate - 비동기로 만들면 스레드 사라짐
        /// </summary>
        /// <returns></returns>
        public abstract CrossPlatform.Infrastructure.Models.GeocoordinatePCL GetGeocoordinate();

        /// <summary>
        /// 위치 기본 값을 반환 - 한국외의 지역에서 이 앱을 사용하는 경우 서울 시청을 기준으로 거리를 계산한다.
        /// </summary>
        /// <returns></returns>
        public abstract CrossPlatform.Infrastructure.Models.GeocoordinatePCL GetDefaultGeocoordinate();

        /// <summary>
        /// GetCivicAddress
        /// </summary>
        /// <returns></returns>
        public abstract CrossPlatform.Infrastructure.Models.CivicAddressPCL GetCivicAddress();

        /// <summary>
        /// 초기화
        /// </summary>
        public abstract void Init();
    }
}
