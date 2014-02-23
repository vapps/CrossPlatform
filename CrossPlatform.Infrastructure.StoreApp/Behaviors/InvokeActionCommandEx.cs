using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace CrossPlatform.Infrastructure.StoreApp.Behaviors
{
    public class InvokeActionCommandEx : DependencyObject, Microsoft.Xaml.Interactivity.IAction
    {
        #region Static Fields

        /// <summary>
        /// Defines the CommandParameter dependency property, of type <see cref="object"/>.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(InvokeActionCommandEx),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the Command dependency property, of type <see cref="ICommand"/>.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(InvokeActionCommandEx),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the Command dependency property, of type <see cref="PassEventArgsToCommand"/>.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsToCommandProperty = DependencyProperty.Register(
            "PassEventArgsToCommand",
            typeof(bool),
            typeof(InvokeActionCommandEx),
            new PropertyMetadata(false));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the command to be invoked.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command parameter to pass to the <see cref="ICommand"/> upon invocation.
        /// </summary>
        /// <remarks>
        /// This takes precedence over the <see cref="PassEventArgsToCommand"/> property - if <see cref="CommandParameter"/>
        /// is specified, then <see cref="PassEventArgsToCommand"/> is ignored.
        /// </remarks>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the event arguments associated to the raised
        /// event should be passed to the command.
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get
            {
                return (bool)this.GetValue(PassEventArgsToCommandProperty);
            }

            set
            {
                this.SetValue(PassEventArgsToCommandProperty, value);
            }
        }
        #endregion

        public object Execute(object sender, object parameter)
        {
            FrameworkElement element = sender as FrameworkElement;

            if (element != null && Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter == null && PassEventArgsToCommand == true ? parameter : this.CommandParameter);
            }

            return null;
        }
    }
}
