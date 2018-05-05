using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaboxRemote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Debug.Assert(this.Frame.CanGoBack);
            this.Frame.GoBack();
        }

        private void OnConnectClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), new MainPageArgument() {
                Command = MainPageCommand.CONNECT,
                Data = ((TextBox)this.FindName("txtAddress")).Text
            });
        }
    }
}
