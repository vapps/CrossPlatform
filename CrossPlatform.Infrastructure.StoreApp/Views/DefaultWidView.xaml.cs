using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace CrossPlatform.Infrastructure.StoreApp.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DefaultWideView : Page
    {
        private ICommand _enterKeyDownCommand;

        public DefaultWideView()
        {
            InitializeComponent();

            if (DesignMode.DesignModeEnabled)
            {
                Title = "InputBox Title";
                Message = "Enter a name for the folder";
                OkText = "OK";
                CancelText = "Cancel";
            }
        }

        public DefaultWideView(string title = "InputBox", string message = "Input Text", string ok = "OK",
            string cancel = "Cancel")
            : this()
        {
            Title = title;
            Message = message;
            OkText = ok;
            CancelText = cancel;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OnClose(tbInput.Text.Trim());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnClose(string.Empty);
        }

        private async void OnClose(string inputText)
        {
            //Close ScreenKeyboard
            btOK.Focus(FocusState.Programmatic);
            await Task.Delay(10);

            var popup = Parent as Popup;
            if (popup != null)
            {
                popup.Tag = inputText;
                popup.IsOpen = false;
            }
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            tbInput.Focus(FocusState.Programmatic);
        }

        public ICommand EnterKeyDownCommand
        {
            get { return _enterKeyDownCommand = _enterKeyDownCommand = new DelegateCommand(args => OnClose((tbInput.Text.Trim()))); }
        }
    }
}