using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure
{
    public abstract class ViewUtility
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static ViewUtility Instance { get; set; }

        /// <summary>
        /// Get current window bounds
        /// </summary>
        /// <returns></returns>
        public abstract RectMini GetWindowBounds();

        /// <summary>
        /// Get Current View
        /// </summary>
        /// <returns></returns>
        public abstract object GetCurrentVeiw();

        /// <summary>
        /// Object의 크기를 반환
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract RectMini GetObjectBounds(object obj);

    }
}
