using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure.StoreApp.Converters
{
    /// <summary>
    /// 데이터와 비교해서 bool값 반환, 비교할 데이터|nor, rev
    /// </summary>
    public class AnyDataToBoolconverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnValue = false;
            var data = value == null ? "" : value.ToString();
            var para = parameter as string;

            if (para == null) return returnValue;

            var split = para.Split('|');
            if (split.Count() != 2) return returnValue;

            if (data == split[0])
            {
                returnValue = split[1] == "nor" ? true : false;
            }
            else
            {
                returnValue = split[1] == "nor" ? false : true;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
