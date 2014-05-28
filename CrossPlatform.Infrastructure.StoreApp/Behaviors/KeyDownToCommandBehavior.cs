using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace CrossPlatform.Infrastructure.StoreApp.Behaviors
{
    public class KeyDownToCommandBehavior : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof (ICommand), typeof (KeyDownToCommandBehavior),
                new PropertyMetadata(null));

        /// <summary>
        ///     Event Key Name
        /// </summary>
        public string EventKeyName { get; set; }

        /// <summary>
        ///     Command
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

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
                    throw new InvalidOperationException("Cannot attach behavior you must have GridView.");
                }
                //SelectedItems가 Runtime Object라서 IObservableVector<object>로 변환해서 VectorChanged를 사용
                control.KeyDown += control_KeyDown;
            }
        }

        public void Detach()
        {
            if (AssociatedObject != null)
            {
                ((FrameworkElement) AssociatedObject).KeyDown -= control_KeyDown;
            }
            AssociatedObject = null;
        }

        public DependencyObject AssociatedObject { get; private set; }

        private void control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EventKeyName)) return;
            if (Command == null) return;

            var key = (VirtualKey) Enum.Parse(typeof (VirtualKey), EventKeyName);
            if (e.Key == key)
            {
                Command.Execute(key);
            }
        }
    }
}