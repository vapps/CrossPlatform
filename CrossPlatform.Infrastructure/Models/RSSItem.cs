﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class RSSItem : BindableBase
    {
        public string id { get; set; }

        public string title { get; set; }

        public string link { get; set; }

        public string summary { get; set; }

        public string content { get; set; }

        public string description { get; set; }

        public string category { get; set; }

        public DateTime pubdate { get; set; }

        public DateTime update { get; set; }

        public string Image_url { get; set; }

        public string Image_title { get; set; }

        public string Image_link { get; set; }
    }
}
