using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.AbstractControls
{
    public abstract class Selecter<T>
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static Selecter<T> Instance { get; set; }

        public Action CoreAction { get; set; }

        public IList<object> Templates { get; set; }
    }
}
