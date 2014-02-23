using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class RSSImage : BindableBase
    {
        private string _url;
        public string url 
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        public string title { get; set; }

        public string link { get; set; }
    }
}
