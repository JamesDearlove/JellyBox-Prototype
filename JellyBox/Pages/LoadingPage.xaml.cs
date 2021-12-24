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

namespace JellyBox.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        public LoadingPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var authResult = await CheckAuthValid();

            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            if (authResult)
            {
                this.Frame.Navigate(typeof(HomePage));
            }
            else
            {
                this.Frame.Navigate(typeof(ServerAddressPage));
            }

            this.Frame.BackStack.Clear();
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
                    return await ShowError("Failed to login to server", ex.Message);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return await ShowError("Unable to reach server", ex.Message);
                }
            }

            return authValid;
        }

        private async Task<bool> ShowError(string title, string message)
        {
            var cantLoginDialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Try again",
                SecondaryButtonText = "Change server",
            };

            var result = await cantLoginDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                return await CheckAuthValid();
            }
            else
            {
                return false;
            }
        }
    }
}
