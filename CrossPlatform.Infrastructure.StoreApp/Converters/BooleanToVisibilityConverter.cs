using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CrossPlatform.Infrastructure.StoreApp.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility returnValue = Visibility.Collapsed;

            if (value != null)
            {
                bool data = (bool)value;
                if (parameter.ToString() == "nor")
                {
                    //기본 : True 보이기, False 숨기기
                    returnValue = data == true ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    //rev : True 숨기기, False 보이기
                    returnValue = data == true ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
