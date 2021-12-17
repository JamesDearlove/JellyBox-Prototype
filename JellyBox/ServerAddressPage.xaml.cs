using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace JellyBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServerAddressPage : Page

    {
        public ServerAddressPage()
        {
            this.InitializeComponent();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestPage));
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }

        private void ServerAddressBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ConnectToServer();
            }
        }

        private async void ConnectToServer()
        {
            // TODO: Refactor this with DataContext, a function for connecting state, and non UI blocking code 
            ConnectButton.IsEnabled = false;
            ConnectingRing.IsActive = true;
            var serverUri = ServerAddressBox.Text;

            if (!serverUri.ToLower().StartsWith("http"))
            {
                serverUri = "http://" + serverUri;
            }

            PublicSystemInfo connectionResult;
            try
            {
                connectionResult = await JellyBox.Core.JellyfinInstance.ConnectServer(serverUri);
            }
            catch (Exception ex)
            {
                ErrorTextblock.Text = ex.Message;
                ConnectButton.IsEnabled = true;
                ConnectingRing.IsActive = false;
                return;
            }

            Core.SettingManager.Server = serverUri;

            ConnectButton.IsEnabled = true;
            ConnectingRing.IsActive = false;
            Frame.Navigate(typeof(ServerLoginPage), $"{connectionResult.LocalAddress} - {connectionResult.ServerName}");
        }
    }
}
