using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppViewUtility : ViewUtility
    {
        public override RectMini GetWindowBounds()
        {
            RectMini returnValue = new RectMini();

            var rect = Windows.UI.Xaml.Window.Current.Bounds;
            if (rect != null)
            {
                returnValue.Bottom = rect.Bottom;
                returnValue.Height = rect.Height;
                returnValue.Left = rect.Left;
                returnValue.Right = rect.Right;
                returnValue.Top = rect.Top;
                returnValue.Width = rect.Width;
            }

            return returnValue;
        }

        public override object GetCurrentVeiw()
        {
            var frame = (Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content;
            var page = (Windows.UI.Xaml.Controls.Page)frame.Content;
            return page;
        }

        public override RectMini GetObjectBounds(object obj)
        {
            RectMini returnValue = null;
            var element = obj as FrameworkElement;
            if (element != null)
            {
                GeneralTransform buttonTransform = element.TransformToVisual(null);
                Point point = buttonTransform.TransformPoint(new Point());
                returnValue = new RectMini 
                    {
                        Left = point.X,
                        Top = point.Y,
                        Width = element.ActualWidth,
                        Height = element.ActualHeight,
                        Right = point.X + element.ActualWidth,
                        Bottom = point.Y + element.ActualHeight
                    };
            }
            return returnValue;
        }
    }
}
