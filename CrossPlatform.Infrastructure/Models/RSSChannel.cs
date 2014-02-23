using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class RSSChannel : BindableBase
    {
        public string title { get; set; }

        public string link { get; set; }

        public string description { get; set;}

        public DateTime pubdate { get; set; }

        public string language { get; set; }

        public string copyright { get; set; }

        public string webmaster { get; set; }

        public string generator { get; set; }

        public string docs { get; set; }

        public int ttl { get; set; }

        public RSSImage image { get; set; }

        public RSSItem currentitem { get; set; }

        public ObservableCollection<RSSItem> items;
    }

}
