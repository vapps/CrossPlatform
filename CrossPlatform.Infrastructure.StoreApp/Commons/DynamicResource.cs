using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    /// <summary>
    /// 다이나믹 리소스
    /// </summary>
    public class DynamicResource : DynamicObject
    {
        /// <summary>
        /// 윈도우 리소스로더
        /// </summary>
        Windows.ApplicationModel.Resources.ResourceLoader _rl;

        /// <summary>
        /// 리소스 전체 이름
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 이름으로 호출
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string str = string.Empty;
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled == false)
            {
                if (_rl == null)
                {
                    _rl = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView(ResourceName);
                }

                str = _rl.GetString(binder.Name);
                if (string.IsNullOrEmpty(str) == true)
                {
                    str = string.Empty;
                }
            }
            else
            {
                str = binder.Name;
            }
            result = str;
            return true;
        }

        /// <summary>
        /// 프로퍼티로 호출
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string this[string id]
        {
            get
            {
                string str = string.Empty;
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled == false)
                {
                    if (_rl == null)
                    {
                        _rl = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView(ResourceName);
                    }

                    str = _rl.GetString(id);
                    if (string.IsNullOrEmpty(str) == true)
                    {
                        str = string.Empty;
                    }
                }
                else
                {   
                    //디자인 타임에서는 키값을 반환
                    str = id;
                }
                return str;
            }
        }
    }
}
