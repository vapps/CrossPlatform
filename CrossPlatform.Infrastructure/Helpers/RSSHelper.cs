using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrossPlatform.Infrastructure
{
    public class RSSHelper : BindableBase
    {
        private static RSSHelper _instance;
        /// <summary>
        /// Instance
        /// </summary>
        public static RSSHelper Instance
        {
            get
            {
                return _instance = _instance ?? new RSSHelper();
            }
        }

        private bool _isBusy;
        /// <summary>
        /// 작업중 여부
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        /// <summary>
        /// Get Rss Channel(Feed) Data
        /// </summary>
        /// <param name="channelUrl"></param>
        /// <returns></returns>
        public async Task<RSSChannel> GetRSSChannel(string channelUrl)
        {
            RSSChannel returnValue = null;

            IsBusy = true;
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var result = await hc.GetAsync(channelUrl);
                    if (result != null && result.IsSuccessStatusCode == true)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        if (data != null)
                        {
                            //xml을 xElement라는 객체로 바로 파싱해서 사용한다.
                            XElement xmlRSS = XElement.Parse(data);

                            var q1 = from rss in xmlRSS.Descendants("channel")
                                     let hasImage = string.IsNullOrEmpty(StaticFunctions.GetString(rss.Element("image"))) == true ? false : true
                                     select new RSSChannel()
                                     {
                                         title = StaticFunctions.GetString(rss.Element("title")),
                                         link = StaticFunctions.GetString(rss.Element("link")),
                                         description = StaticFunctions.GetString(rss.Element("description")),
                                         pubdate = StaticFunctions.GetDateTime(rss.Element("pubDate")),
                                         language = StaticFunctions.GetString(rss.Element("language")),
                                         copyright = StaticFunctions.GetString(rss.Element("copyright")),
                                         webmaster = StaticFunctions.GetString(rss.Element("webMaster")),
                                         generator = StaticFunctions.GetString(rss.Element("generator")),
                                         docs = StaticFunctions.GetString(rss.Element("docs")),
                                         ttl = StaticFunctions.GetInt(rss.Element("ttl")),
                                         image = hasImage ? new RSSImage()
                                         {
                                             url = StaticFunctions.GetString(rss.Element("image").Element("url")),
                                             title = StaticFunctions.GetString(rss.Element("image").Element("title")),
                                             link = StaticFunctions.GetString(rss.Element("image").Element("link")),
                                         } : null,
                                         items = new System.Collections.ObjectModel.ObservableCollection<RSSItem>()
                                     };

                            var q2 = from item in xmlRSS.Descendants("item")
                                     let hasImage = string.IsNullOrEmpty(StaticFunctions.GetString(item.Element("image"))) == true ? false : true
                                     select new RSSItem()
                                     {
                                         title = StaticFunctions.GetString(item.Element("title")),
                                         link = StaticFunctions.GetString(item.Element("link")),
                                         description = StaticFunctions.GetString(item.Element("description")),
                                         category = StaticFunctions.GetString(item.Element("category")),
                                         pubdate = StaticFunctions.GetDateTime(item.Element("pubDate")),
                                         Image_link = hasImage ? StaticFunctions.GetString(item.Element("image").Element("link")) : StaticFunctions.GetString(item.Element("link")),
                                         Image_title = hasImage ? StaticFunctions.GetString(item.Element("image").Element("title")) : StaticFunctions.GetString(item.Element("title")),
                                         Image_url = hasImage ? StaticFunctions.GetString(item.Element("image").Element("url")) : null,
                                     };

                            if (q1.Count() > 0)
                            {
                                returnValue = q1.First();
                                foreach (var item in q2)
                                {
                                    returnValue.items.Add(item);
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            IsBusy = false;
            return returnValue;
        }

        /// <summary>
        /// Get Rss Channel(Feed) Data
        /// </summary>
        /// <param name="channelUrl"></param>
        /// <returns></returns>
        public async Task<RSSChannel> GetRSSFeed(string channelUrl)
        {
            RSSChannel returnValue = null;

            IsBusy = true;
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var result = await hc.GetAsync(channelUrl);
                    if (result != null && result.IsSuccessStatusCode == true)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        if (data != null)
                        {
                            //xml을 xElement라는 객체로 바로 파싱해서 사용한다.
                            XDocument xmlRSS = XDocument.Parse(data);

                            XNamespace ns = "http://www.w3.org/2005/Atom";

                            var q1 = from rss in xmlRSS.Descendants(ns + "feed")
                                     let hasImage = string.IsNullOrEmpty(StaticFunctions.GetString(rss.Element(ns + "image"))) == true ? false : true
                                     select new RSSChannel()
                                     {
                                         author = StaticFunctions.GetString(rss.Element(ns + "author")),
                                         title = StaticFunctions.GetString(rss.Element(ns + "title")),
                                         link = StaticFunctions.GetStringAttribute(rss.Element(ns + "link"), "href"),
                                         description = StaticFunctions.GetString(rss.Element(ns + "description")),
                                         pubdate = StaticFunctions.GetDateTime(rss.Element(ns + "pubDate")),
                                         language = StaticFunctions.GetString(rss.Element(ns + "language")),
                                         copyright = StaticFunctions.GetString(rss.Element(ns + "copyright")),
                                         webmaster = StaticFunctions.GetString(rss.Element(ns + "webMaster")),
                                         generator = StaticFunctions.GetString(rss.Element(ns + "generator")),
                                         docs = StaticFunctions.GetString(rss.Element(ns + "docs")),
                                         id = StaticFunctions.GetString(rss.Element(ns + "id")),
                                         ttl = StaticFunctions.GetInt(rss.Element(ns + "ttl")),
                                         image = hasImage ? new RSSImage()
                                         {
                                             url = StaticFunctions.GetString(rss.Element(ns + "image").Element(ns + "url")),
                                             title = StaticFunctions.GetString(rss.Element(ns + "image").Element(ns + "title")),
                                             link = StaticFunctions.GetString(rss.Element(ns + "image").Element(ns + "link")),
                                         } : null,
                                         items = new System.Collections.ObjectModel.ObservableCollection<RSSItem>()
                                     };

                            var q2 = from item in xmlRSS.Descendants(ns + "entry")
                                     let hasImage = string.IsNullOrEmpty(StaticFunctions.GetString(item.Element(ns + "image"))) == true ? false : true
                                     select new RSSItem()
                                     {
                                         title = StaticFunctions.GetString(item.Element(ns + "title")),
                                         pubdate = StaticFunctions.GetDateTime(item.Element(ns + "published")),
                                         update = StaticFunctions.GetDateTime(item.Element(ns + "updated")),
                                         id = StaticFunctions.GetString(item.Element(ns + "id")),
                                         link = StaticFunctions.GetStringAttribute(item.Element(ns + "link"), "href"),
                                         summary = StaticFunctions.GetString(item.Element(ns + "summary")),
                                         content = StaticFunctions.GetHtml(item.Element(ns + "content")),
                                         description = StaticFunctions.GetString(item.Element(ns + "description")),
                                         category = StaticFunctions.GetString(item.Element(ns + "category")),
                                         Image_link = hasImage ? StaticFunctions.GetString(item.Element(ns + "image").Element(ns + "link")) : StaticFunctions.GetString(item.Element(ns + "link")),
                                         Image_title = hasImage ? StaticFunctions.GetString(item.Element(ns + "image").Element(ns + "title")) : StaticFunctions.GetString(item.Element(ns + "title")),
                                         Image_url = hasImage ? StaticFunctions.GetString(item.Element(ns + "image").Element(ns + "url")) : null,
                                     };

                            if (q1.Count() > 0)
                            {
                                returnValue = q1.First();
                                foreach (var item in q2)
                                {
                                    returnValue.items.Add(item);
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            IsBusy = false;
            return returnValue;
        }
    }
}
