using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public class LiveSDKHelper
    {
        private static LiveSDKHelper instance;
        public static LiveSDKHelper Instance 
        { 
            get
            {
                if(instance == null)
                {
                    instance = new LiveSDKHelper();
                }
                return instance;
            }
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="scopes"></param>
        //public async Task<bool> LoginAsync(IList<string> scopes)
        //{ 

        //}

        /// <summary>
        /// 라이브아이디 로그인 사용자 정보
        /// </summary>
        public LiveUser LiveLoginUser { get; set; }

        /// <summary>
        /// 로그아웃 가능 여부
        /// </summary>
        public bool IsSignOut { get; set; }
    }
}
