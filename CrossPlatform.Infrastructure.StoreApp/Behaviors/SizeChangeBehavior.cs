using System;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace CrossPlatform.Infrastructure.StoreApp.Behaviors
{
    public class ScreenKeyboardBehavior : DependencyObject, IBehavior
    {
        private Rect orignalRect;
        private InputPane popupInputPane;

        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !DesignMode.DesignModeEnabled)
            {
                if (AssociatedObject != null)
                    throw new InvalidOperationException("Cannot attach behavior multiple times.");

                AssociatedObject = associatedObject;
                var control = AssociatedObject as FrameworkElement;
                if (control == null)
                {
                    throw new InvalidOperationException("Cannot attach behavior you must have Control.");
                }
                popupInputPane = InputPane.GetForCurrentView();
                popupInputPane.Showing += popupInputPane_Showing;
                popupInputPane.Hiding += popupInputPane_Hiding;
                Window.Current.SizeChanged += Current_SizeChanged;

                var popup = ((FrameworkElement) AssociatedObject).Parent as Popup;
                if (popup == null) return;
                //키보드가 보인다는 이야기
                orignalRect = new Rect
                {
                    X = popup.HorizontalOffset,
                    Y = popup.VerticalOffset,
                    Width = ((FrameworkElement) AssociatedObject).Width,
                    Height = ((FrameworkElement) AssociatedObject).Height
                };
                resizePopup();
            }
        }

        public void Detach()
        {
            if (AssociatedObject != null)
            {
                Window.Current.SizeChanged -= Current_SizeChanged;
                popupInputPane.Showing -= popupInputPane_Showing;
                popupInputPane.Hiding -= popupInputPane_Hiding;
            }
            AssociatedObject = null;
            popupInputPane = null;
        }

        public DependencyObject AssociatedObject { get; private set; }

        private void resizePopup()
        {
            Rect inputPanelRect = popupInputPane.OccludedRect;
            Rect rect = Window.Current.CoreWindow.Bounds;
            //키보드 뜰때 세로만 변경
            var popup = ((FrameworkElement) AssociatedObject).Parent as Popup;
            if (popup == null) return;

            if (Math.Abs(inputPanelRect.Top) < 1)
            {
                //키보드가 숨었다는 이야기
                popup.VerticalOffset = orignalRect.Top;
                ((FrameworkElement) AssociatedObject).Height = orignalRect.Height;
            }
            else
            {
                //화면높이에서 키보드영역 크기만큼 빼고
                rect.Height -= inputPanelRect.Height;

                if (Math.Abs(popup.VerticalOffset) < 1 && Math.Abs(popup.HorizontalOffset) < 1)
                {
                    //전체 화면
                    ((FrameworkElement) AssociatedObject).Height = rect.Height;
                }
                else
                {
                    //일부 화면
                    popup.VerticalOffset = rect.Top + 45;
                    //팝업 높이 화면 크기 만큼
                    ((FrameworkElement) AssociatedObject).Height = rect.Height - 90;
                }
            }
        }

        private void popupInputPane_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            resizePopup();
        }

        private void popupInputPane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            resizePopup();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            resizePopup();
        }
    }
}