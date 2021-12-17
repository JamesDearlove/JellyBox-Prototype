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
    public sealed partial class ServerLoginPage : Page
    {
        private string ServerName { get; set; }

        public ServerLoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Replace with DataContext
            ServerName = (string)e.Parameter;
            ServerAddressText.Text = ServerName;
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
                ErrorTextblock.Text = ex.Message;
                return;
            }

            Core.SettingManager.Username = authResult.User.Name;
            Core.SettingManager.AccessToken = authResult.AccessToken;

            Frame.Navigate(typeof(HomePage));
        }
    }
}
