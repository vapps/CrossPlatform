using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CrossPlatform.Infrastructure.Silverlight
{
    public class SilverlightEtcUtility : EtcUtility
    {
        public override void MsgBox(string content)
        {
            MessageBox.Show(content);
        }

        public override async Task<bool> ConfirmAsync(string context, string title = "Confirm")
        {
            MessageBoxResult result = MessageBoxResult.Cancel;
            result = MessageBox.Show(context, title, MessageBoxButton.OKCancel);
            await TaskEx.Delay(1);      //단지 async를 사용하기 위한..
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
