using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jellyfin.Sdk;

namespace JellyBox
{
    public static class Core
    {
        public static JellyfinInstance JellyfinInstance = new JellyfinInstance();
    }

    public class JellyfinInstance
    {
        private SdkClientSettings sdkClientSettings = new SdkClientSettings();
        
        private ISystemClient systemClient;
        private IUserClient userClient;
        private IUserViewsClient userViewsClient;
        private IUserLibraryClient userLibraryClient;

        private HttpClient httpClient = new HttpClient();

        public UserDto LoggedInUser { get; set; }

        public JellyfinInstance()
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

        public Task<PublicSystemInfo> ConnectServer(string serverUri)
        {
            sdkClientSettings.BaseUrl = serverUri;

            return systemClient.GetPublicSystemInfoAsync();
        }

        public async Task<AuthenticationResult> AuthenticateUser(string username, string password)
        {
            var authResult = await userClient.AuthenticateUserByNameAsync(new AuthenticateUserByName { Username = username, Pw = password });

            sdkClientSettings.AccessToken = authResult.AccessToken;

            LoggedInUser = authResult.User;

            return authResult;
        }

        /// <summary>
        /// Gets user views for the logged in user. Fails if the user is not logged in
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<BaseItemDto>> GetUserViews()
        {
            return (await userViewsClient.GetUserViewsAsync(LoggedInUser.Id)).Items;
        }

        public async Task<IReadOnlyList<BaseItemDto>> GetView(string id)
        {
            return null;
        }

        public async Task<IReadOnlyList<BaseItemDto>> GetLatestMedia()
        {
            return (await userLibraryClient.GetLatestMediaAsync(LoggedInUser.Id));
        }
    }
}
