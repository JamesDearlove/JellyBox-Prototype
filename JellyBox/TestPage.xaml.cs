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

using Jellyfin.Sdk;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JellyBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        SdkClientSettings sdkClientSettings = new SdkClientSettings();
        ISystemClient systemClient;
        IUserClient userClient;
        IUserViewsClient userViewsClient;
        IUserLibraryClient userLibraryClient;

        HttpClient httpClient = new HttpClient();

        private void Page_Loading(FrameworkElement sender, object args)
        {
            // Configure Jellyfin SDK
            sdkClientSettings.InitializeClientSettings(
                "JellyBox",
                "0.0.1",
                "Xbox",
                $"xbox-jellybox"
            );

            // Configure HTTP Client
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    "JellyBox",
                    "0.0.1"
                )
            );

            // For what ever reason json doesnt exist. Manually working around
            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json, 1.0));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

            // Configuring other clients
            systemClient = new SystemClient(sdkClientSettings, httpClient);
            userClient = new UserClient(sdkClientSettings, httpClient);
            userViewsClient = new UserViewsClient(sdkClientSettings, httpClient);
            userLibraryClient = new UserLibraryClient(sdkClientSettings, httpClient);
        }

        private async void LoginBox_Click(object sender, RoutedEventArgs e)
        {
            var resultString = "";

            try
            {
                // Verify server URL is valid first
                var host = AddressBox.Text;
                sdkClientSettings.BaseUrl = host;

                // Sample also had this: .ConfigureAwait(false);
                var systemInfo = await systemClient.GetPublicSystemInfoAsync();
                resultString += $"Connected to {host}\n";
                resultString += $"Server name: {systemInfo.ServerName}\n";
                resultString += $"Version number: {systemInfo.Version}\n";

                var authResult = await userClient.AuthenticateUserByNameAsync(new AuthenticateUserByName { Username = UsernameBox.Text, Pw = PasswordBox.Password });
                sdkClientSettings.AccessToken = authResult.AccessToken;

                var user = authResult.User;
                resultString += "\n";
                resultString += $"Logged in as: {user.Name}\n";
                ErrorTextblock.Text = "";   

                var views = await userViewsClient.GetUserViewsAsync(user.Id);
                resultString += "\n";
                resultString += "Views: \n";
                foreach (var view in views.Items)
                {
                    resultString += $"{view.Id} - {view.Name}\n";
                }

                var library = await userLibraryClient.GetLatestMediaAsync(user.Id);
                resultString += "\n";
                resultString += "Latest Media: \n";
                foreach (var item in library)
                {
                    resultString += $"{item.Id} - {item.Name}\n";
                }
            }
            catch (Exception ex)
            {
                ErrorTextblock.Text = ex.Message;
            }

            SuccessTextblock.Text = resultString;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
