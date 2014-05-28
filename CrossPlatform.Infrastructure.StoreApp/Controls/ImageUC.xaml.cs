using CrossPlatform.Infrastructure.StoreApp.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace CrossPlatform.Infrastructure.StoreApp.Controls
{
    public sealed partial class ImageUC : UserControl
    {
        public ImageUC()
        {
            this.InitializeComponent();
        }

        public ICommand ImageFailedCommand
        {
            get { return (ICommand)GetValue(ImageFailedCommandProperty); }
            set { SetValue(ImageFailedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _imageFailCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageFailedCommandProperty =
            DependencyProperty.Register("ImageFailedCommand", typeof(ICommand), typeof(ImageUC), new PropertyMetadata(null));

        public static readonly DependencyProperty PictureUriStringProperty =
            DependencyProperty.Register
            (
                "PictureUriString",
                typeof(string),
                typeof(ImageUC),
                new PropertyMetadata(null, 
                    (d, e) => 
                    {
                        var sender = (ImageUC)d;
                        sender.PictureUriChanged(e.OldValue, e.NewValue);
                    }));

        public string PictureUriString
        {
            get { return (string)GetValue(PictureUriStringProperty); }
            set { SetValue(PictureUriStringProperty, value); }
        }

        public void PictureUriChanged(object oldValue, object newValue)
        {
            if (IsNoImageUse == true)
                noImage.Visibility = Visibility.Visible;

            if (newValue != null && string.IsNullOrEmpty(newValue as string) == false)
            {
                var imageUri = (string)newValue;
                //웹이미지이면..
                if(imageUri.Contains("http://") == true)
                {
                    GetLocalImage(imageUri);
                }
                else
                {
                    var bi = new BitmapImage();
                    bi.UriSource = new Uri(imageUri);
                    imagePic.Source = bi;
                    imagePic.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    //noImage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }

        private async void GetLocalImage(string imageUri)
        {
            imagePic.Source = await StaticFunctionsStore.Instance.UriImageSaveLocalAsync(imageUri);
            imagePic.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public static readonly DependencyProperty IsNoImageUseParameterProperty =
            DependencyProperty.Register
            (
                "IsNoImageUse",
                typeof(bool),
                typeof(ImageUC),
                new PropertyMetadata(true,
                    (d, e) =>
                    {
                        if (((bool)e.NewValue) == true)
                        {
                            ((ImageUC)d).noImage.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ((ImageUC)d).noImage.Visibility = Visibility.Collapsed;
                        }
                    }));

        public bool IsNoImageUse
        {
            get { return (bool)GetValue(IsNoImageUseParameterProperty); }
            set { SetValue(IsNoImageUseParameterProperty, value); }
        }

    }
}
