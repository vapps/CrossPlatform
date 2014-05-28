using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp.Converters
{
    /// <summary>
    /// 파라메터에 데이터 반드시 필요
    /// 파마메터 구성 : 비교값|nor or rev, nor:정상 비교값과 같으면 보여주고, 아니면 숨기고, rev:nor과 반대 작용
    /// </summary>
    public class AnyDataToVisibilityConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnValue = Visibility.Collapsed;
            var data = value == null ? "" : value.ToString();
            var para = parameter as string;

            if (para == null) return returnValue;

            var split = para.Split('|');
            if (split.Count() != 2) return returnValue;

            if (data == split[0])
            {
                returnValue = split[1] == "nor" ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                returnValue = split[1] == "nor" ? Visibility.Collapsed : Visibility.Visible;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
