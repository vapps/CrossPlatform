using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Resources;

namespace CrossPlatform.Infrastructure.Phone
{
    public class PhoneEtcUtility : EtcUtility
    {
        public override void MsgBox(string content)
        {
            MessageBox.Show(content);
        }

        public override async Task<bool> ConfirmAsync(string context, string title = "Confirm")
        {
            MessageBoxResult result = MessageBoxResult.Cancel;
            await TaskEx.Run(
                () =>
                {
                    StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                        para =>
                        {
                            result = MessageBox.Show(context, title, MessageBoxButton.OKCancel);
                        }, null);
                });
            return result == MessageBoxResult.OK ? true : false;
        }

        public override T GetPropertyValue<T>(object source, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override Interfaces.ICommonILC GetILC(System.Collections.IList source, Func<Task> loadDataCallBack)
        {
            throw new NotImplementedException();
        }

        public override bool SetPropertyValue(object source, string propertyName, object setValue)
        {
            throw new NotImplementedException();
        }

        public override bool IsExistProperty(object source, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> LaunchUriAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> LaunchUriAsync(Uri uri, object options)
        {
            throw new NotImplementedException();
        }

        public override void ToastShow(string body, string header = null)
        {
            throw new NotImplementedException();
        }

        public override object GetPropertyValue(object source, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override bool GetAvaliableConnection()
        {
            throw new NotImplementedException();
        }
    }
}
