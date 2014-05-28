using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CrossPlatform.Infrastructure
{
    public static class StaticFunctions
    {   
        /// <summary>
        //          StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
        //              para =>
        //              {
        //              }, null);
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public static void InvokeIfRequiredAsync(SynchronizationContext context, SendOrPostCallback callback, object state)
        {
            context.Post(callback, state); //모든 프레임웍에서 동작할려면 이거 써야한다는데..
        }

        /// <summary>
        /// 모델 뷰모델 모두 사용 - 기본 컨텍스트
        /// </summary>
        public static System.Threading.SynchronizationContext BaseContext { get; set; }

        /// <summary>
        /// 숫자로
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static int GetInt(object element)
        {
            int returnValue = -1;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = Convert.ToInt32(string.IsNullOrEmpty(x.Value) ? "0" : x.Value);
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 문자로
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetString(object element)
        {
            string returnValue = string.Empty;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = x.Value.Trim();
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 날짜로
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object element)
        {
            DateTime returnValue = DateTime.MinValue;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        int idate;
                        if (int.TryParse(x.Value, out idate))
                        {
                            returnValue = DateTime.Parse(idate.ToString("####-##-##")).ToLocalTime();
                        }
                        else
                        {
                            returnValue = DateTime.Parse(x.Value).ToLocalTime();
                        }
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 더블형
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetDouble(object element)
        {
            double returnValue = -1;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = Convert.ToDouble(x.Value);
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 싱글형
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Single GetSingle(object element)
        {
            Single returnValue = -1;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = Convert.ToSingle(x.Value);
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// url
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Uri GetUri(object element)
        {
            string returnValue = string.Empty;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = x.Value.Trim();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(returnValue))
            {
                return new Uri(returnValue);
            }

            return null;
        }

        /// <summary>
        /// 문자열 속성 반환
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetStringAttribute(object element, string attributeName)
        {
            string returnValue = string.Empty;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        if (x.Attribute(attributeName) != null)
                            returnValue = x.Attribute(attributeName).Value;
                        break;
                }
            }
            return returnValue;
        }


        public static string GetHtml(object element)
        {
            string returnValue = string.Empty;

            if (element != null)
            {
                switch (element.GetType().Name)
                {
                    case "XElement":
                        XElement x = element as XElement;
                        returnValue = x.FirstNode.ToString();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(returnValue))
            {
                return returnValue;
            }

            return null;
        }

        public static T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException iox)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return returnedXmlClass;
        }

        /// <summary>
        /// 모든 테그 삭제
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RemoveTag(string data)
        {
            string returnValue = string.Empty;
            if (data.Length > 0)
            {
                returnValue = System.Text.RegularExpressions.Regex.Replace(data, @"<[^>]+>", String.Empty);      //모든 테그
                //returnValue = Regex.Replace(data, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1").Replace("<br>","");        //a테그만
            }
            return returnValue;
        }

        ///// <summary>
        ///// 색상을 컬러로 바꿔주는 함수 #FFRRGGBB
        ///// </summary>
        ///// <param name="rgbColor"></param>
        ///// <returns></returns>
        //public static Color FromStringColor(string rgbColor)
        //{
        //    Color c = new Color();
        //    //byte a = 255; // or whatever...
        //    byte a = (byte)(Convert.ToUInt32(rgbColor.Substring(1, 2), 16));
        //    byte r = (byte)(Convert.ToUInt32(rgbColor.Substring(3, 2), 16));
        //    byte g = (byte)(Convert.ToUInt32(rgbColor.Substring(5, 2), 16));
        //    byte b = (byte)(Convert.ToUInt32(rgbColor.Substring(7, 2), 16));
        //    c = Color.FromArgb(a, r, g, b);
        //    return c;
        //}

        ///// <summary>
        ///// 문자열 컬러 리턴
        ///// </summary>
        ///// <param name="colorName"></param>
        ///// <returns></returns>
        //public static Color FromKnownColor(string colorName)
        //{
        //    Line lne = (Line)XamlReader.Load("<Line xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Fill=\"" + colorName + "\" />");
        //    return (Color)lne.Fill.GetValue(SolidColorBrush.ColorProperty);
        //}
        /// <summary>
        /// 언어 리소스 리턴 함수
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static string GetHtml(string Head, string Body, bool removeTag = true, bool addATag = false)
        {
            if (Body.Trim().Length == 0)
                return string.Empty;
            Body = Body.Trim();
            Body = Body.Replace("&nbsp;", " ");
            Body = Body.Replace("\n", "").Replace("\r", "");

            if (removeTag)
            {
                Body = StaticFunctions.RemoveTag(Body);
                Body = Body.Replace("<br>", "<br/>").Replace("<BR>", "<br/>").Replace("</a>", "").Replace("&", " ").Replace("*", "<br/>*").Replace("gt;", ">").Replace("※", "<br/>※").Replace("[", "<br/>[").Replace("★", "<br/>★");
                if (Body.StartsWith("<br/>"))
                    Body = Body.Substring(5);
            }
            if (addATag)
            {
                Body = string.Format("<a href='{0}'>{0}</a>", Body);
            }

            string returnValue = string.Format("<div><p><b>{0}</b></p><p>{1}</p></div>", Head, Body);
            return returnValue;
        }

        public static string GetDate(string data)
        {
            string returnValue = string.Empty;
            if (data.Length > 0)
            {
                int intData;
                if (int.TryParse(data, out intData))
                {
                    returnValue = intData.ToString("####-##-##");
                }
                else
                {
                    returnValue = data;
                }
            }
            return returnValue;
        }

        public static string GetObjectSerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static object GetStringSerializeToObject(string str, Type returnType)
        {
            if (string.IsNullOrEmpty(str) == true) return null;
            XmlSerializer serializer = new XmlSerializer(returnType);
            using (StringReader reader = new StringReader(str))
            {
                var obj = serializer.Deserialize(reader);
                return obj;
            }
        }

        ///// <summary>
        ///// 리소스 파일 타입
        ///// </summary>
        //public static Type ResourceType;

        //private static ResourceManager rm;
        ///// <summary>
        ///// 리소스 반환 - ResourceType반드시 지정
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string GetResData(string key)
        //{
        //    var returnValue = string.Empty;
        //    if (ResourceType == null) return returnValue;

        //    rm = rm ?? new ResourceManager(ResourceType);
        //    returnValue = rm.GetString(key);
        //    return returnValue;
        //}

        /// <summary>
        /// DynamicResource object set
        /// </summary>
        public static dynamic DResource;
    }
}
