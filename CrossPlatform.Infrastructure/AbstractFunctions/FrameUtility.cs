using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure
{
    public abstract class FrameUtility
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static FrameUtility Instance { get; set; }

        /// <summary>
        /// 프레임 등록
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="sessionStateKey"></param>
        public abstract void RegisterFrame(object frame, string sessionStateKey = null);

        /// <summary>
        /// Navigation 
        /// </summary>
        /// <param name="navigation">Store : Type, Phone : Uri</param>
        /// <param name="navigationParameter"></param>
        /// <returns></returns>
        public abstract bool Navigation(string navigation, object navigationParameter);

        /// <summary>
        /// 프레임을 이용한 데이터를 저장
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool SetKeepData(string key, object value);

        /// <summary>
        /// 프레임에 저장된 데이터를 조회
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract object GetKeepData(string key);

    }
}
