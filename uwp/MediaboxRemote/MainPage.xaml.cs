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
using Windows.UI.ViewManagement;
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Windows.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediaboxRemote
{
    public enum MainPageCommand
    {
        CONNECT
    }

    public class MainPageArgument
    {
        public MainPageCommand Command;
        public object Data;
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        sealed class CommandHandler : ICommand
        {
            private MainPage m_page;
            public event EventHandler CanExecuteChanged;

            public CommandHandler(MainPage page)
            {
                m_page = page;
            }

            public void RaiseCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }

            bool ICommand.CanExecute(object parameter)
            {
                return m_page.connected;
            }

            void ICommand.Execute(object parameter)
            {
                m_page.SendCommandAsync((string)parameter);
            }
        }

        private bool connected = false;
        private string address = string.Empty;
        private CommandHandler command_handler;
        private StreamSocket socket;

        public MainPage()
        {
            this.InitializeComponent();
            command_handler = new CommandHandler(this);

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 400));
            ApplicationView.PreferredLaunchViewSize = new Size(300, 400);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async void ConnectAsync(string address)
        {
            if (this.connected)
                return;

            bool was_connected = this.connected;

            // connect
            this.address = address;

            if (this.address != string.Empty)
            {
                this.socket = new StreamSocket();
                await this.socket.ConnectAsync(new HostName(this.address), "2048");
                this.connected = true;
                Debug.WriteLine(string.Format("Connecting to {0}", address));
            }
            else
            {
                this.connected = false;
            }

            // update UI
            if (this.connected != was_connected)
                this.command_handler.RaiseCanExecuteChanged();
        }

        private async void SendCommandAsync(string command)
        {
            if (!this.connected)
                return;

            try
            {
                // send the message
                Debug.WriteLine(String.Format("Sending command: {0}", command));

                using (StreamWriter writer = new StreamWriter(this.socket.OutputStream.AsStreamForWrite(),
                    System.Text.Encoding.Default, 1024, true))
                    await writer.WriteLineAsync(command);
            }
            catch (Exception ex)
            {
                bool was_connected = this.connected;
                this.connected = false;
                if (was_connected)
                    this.command_handler.RaiseCanExecuteChanged();
                Debug.WriteLine(ex.ToString());
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.GetType() == typeof(MainPageArgument))
            {
                MainPageArgument arg = (MainPageArgument)e.Parameter;
                if (arg.Command == MainPageCommand.CONNECT)
                {
                    this.ConnectAsync((string)arg.Data);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        private void OnConnectClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ConnectPage), null);
        }

        private void OnAboutClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage), null);
        }

        private void OnShowMenu(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Showing menu");
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private ICommand ClickCommand
        {
            get { return this.command_handler; }
        }

        private void OnKeyboardClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Keyboard button pressed");
        }
    }
}
