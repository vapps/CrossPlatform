using CrossPlatform.Infrastructure.Models;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppViewUtility : ViewUtility
    {
        public override RectMini GetWindowBounds()
        {
            var returnValue = new RectMini();

            var rect = Window.Current.Bounds;
            returnValue.Bottom = rect.Bottom;
            returnValue.Height = rect.Height;
            returnValue.Left = rect.Left;
            returnValue.Right = rect.Right;
            returnValue.Top = rect.Top;
            returnValue.Width = rect.Width;

            return returnValue;
        }

        public override object GetCurrentVeiw()
        {
            var frame = (Windows.UI.Xaml.Controls.Frame)Window.Current.Content;
            var page = (Windows.UI.Xaml.Controls.Page)frame.Content;
            return page;
        }

        public override RectMini GetObjectBounds(object obj)
        {
            RectMini returnValue = null;
            var element = obj as FrameworkElement;
            if (element == null) return null;
            var buttonTransform = element.TransformToVisual(null);
            var point = buttonTransform.TransformPoint(new Point());
            returnValue = new RectMini 
            {
                Left = point.X,
                Top = point.Y,
                Width = element.ActualWidth,
                Height = element.ActualHeight,
                Right = point.X + element.ActualWidth,
                Bottom = point.Y + element.ActualHeight
            };
            return returnValue;
        }

        public override RectMini GetFocusedElement()
        {
            RectMini returnValue = null;
            var ele = Windows.UI.Xaml.Input.FocusManager.GetFocusedElement();
            if (ele != null)
            {
                returnValue = GetObjectBounds(ele);
            }
            return returnValue;
        }
    }
}
