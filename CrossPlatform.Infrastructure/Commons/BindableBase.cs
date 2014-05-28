using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CrossPlatform.Infrastructure
{
    //[DataContract(IsReference = true)]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (StaticFunctions.BaseContext != null)
            {
                StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                    para =>
                    {
                        try
                        {
                            if (PropertyChanged != null)
                            {
                                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                            }
                        }
                        catch (Exception)
                        {
                            System.Diagnostics.Debug.WriteLine("OnPropertyChanged Error");
                        }
                    }, null);
            }
            else if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChangedEx([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}