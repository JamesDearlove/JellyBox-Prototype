using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ServerLoginPage : Page
    {
        private string ServerName { get; set; }

        public ServerLoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Loading screen, disable background.
            // TODO: Store login state (once logout is possible)
            AuthenticationResult authResult;
            try
            {
                authResult = await Core.JellyfinInstance.AuthenticateUser(UsernameBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {
                ErrorLoginTextblock.Text = ex.Message;
                return;
            }

            Core.SettingManager.Username = authResult.User.Name;
            Core.SettingManager.AccessToken = authResult.AccessToken;

            Frame.Navigate(typeof(HomePage));
        }

        private async void SkipLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (await CheckAuthValid())
            {
                Frame.Navigate(typeof(HomePage));
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
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
                ErrorAddressTextblock.Text = "";
            }
            catch (Exception ex)
            {
                ErrorAddressTextblock.Text = ex.Message;
                ConnectButton.IsEnabled = true;
                ConnectingRing.IsActive = false;
                return;
            }

            Core.SettingManager.Server = serverUri;

            ConnectButton.IsEnabled = true;
            ConnectingRing.IsActive = false;

            ServerName = $"{connectionResult.LocalAddress} - {connectionResult.ServerName}";
            ServerAddressText.Text = ServerName;
        }

        /// <summary>
        /// Check if the application stored authentication exists and is valid.
        /// </summary>
        /// <returns>true if it exists and is valid, false otherwise</returns>
        private async Task<bool> CheckAuthValid()
        {
            var serverUrl = Core.SettingManager.Server;
            var accessToken = Core.SettingManager.AccessToken;
            var authValid = false;

            if (serverUrl != null && accessToken != null)
            {
                try
                {
                    var result = await Core.JellyfinInstance.LoadSettings(serverUrl, accessToken);
                    authValid = true;
                }
                catch (Jellyfin.Sdk.SystemException ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorLoginTextblock.Text = "Failed to login to server\n" + ex.Message;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorLoginTextblock.Text = "Unable to reach server\n" + ex.Message;
                }
            }

            return authValid;
        }

    }
}
