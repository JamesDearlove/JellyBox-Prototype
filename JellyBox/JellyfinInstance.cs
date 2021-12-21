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

    public class JellyfinInstance
    {
        private SdkClientSettings sdkClientSettings = new SdkClientSettings();

        private ISystemClient systemClient;
        private IUserClient userClient;
        private IUserViewsClient userViewsClient;
        private IUserLibraryClient userLibraryClient;
        private IImageClient imageClient;
        private IVideosClient videosClient;
        private ITvShowsClient tvShowsClient;

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
            // TODO: Determine the memory impact of these.
            systemClient = new SystemClient(sdkClientSettings, httpClient);
            userClient = new UserClient(sdkClientSettings, httpClient);
            userViewsClient = new UserViewsClient(sdkClientSettings, httpClient);
            userLibraryClient = new UserLibraryClient(sdkClientSettings, httpClient);
            imageClient = new ImageClient(sdkClientSettings, httpClient);
            videosClient = new VideosClient(sdkClientSettings, httpClient);
            tvShowsClient = new TvShowsClient(sdkClientSettings, httpClient);
        }

        public async Task<SystemInfo> LoadSettings(string baseUrl, string accessToken)
        {
            sdkClientSettings.BaseUrl = baseUrl;
            sdkClientSettings.AccessToken = accessToken;

            try
            {
                var systemInfo = await systemClient.GetSystemInfoAsync();
                var loggedInUser = await userClient.GetCurrentUserAsync();

                LoggedInUser = loggedInUser;

                return systemInfo;
            }
            catch (Exception ex)
            {
                // The login failed, revert the sdkClientSettings to be unset
                sdkClientSettings.BaseUrl = null;
                sdkClientSettings.AccessToken = null;
                throw ex;
            }
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

        public void ClearSettings()
        {
            sdkClientSettings.BaseUrl = null;
            sdkClientSettings.AccessToken = null;
        }

        /// <summary>
        /// Gets user views for the logged in user. Fails if the user is not logged in
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<BaseItemDto>> GetUserViews()
        {
            return (await userViewsClient.GetUserViewsAsync(LoggedInUser.Id)).Items;
        }

        //public async Task<IReadOnlyList<BaseItemDto>> GetView(string id)
        //{
        //    return null;
        //}

        public async Task<IReadOnlyList<BaseItemDto>> GetLatestMedia()
        {
            return (await userLibraryClient.GetLatestMediaAsync(LoggedInUser.Id));
        }

        public Task<FileResponse> GetItemImage(Guid id)
        {
            return imageClient.GetItemImageAsync(id, ImageType.Primary);
        }

        public Task<FileResponse> GetItemImage(Guid id, int width, int height)
        {
            return imageClient.GetItemImageAsync(id, ImageType.Primary, width: width, height: height);
        }


        // Video Streaming
        public async Task<FileResponse> GetVideoStream(Guid id)
        {
            //return (await videosClient.GetVideoStreamAsync(id));

            return await videosClient.GetVideoStreamByContainerAsync(id, "ts");

        }

        public async Task<BaseItemDto> GetItem(Guid id)
        {
            return await userLibraryClient.GetItemAsync(LoggedInUser.Id, id);
        }

        public Task<BaseItemDtoQueryResult> GetSeriesSeasons(Guid id)
        {
            return tvShowsClient.GetSeasonsAsync(id);
        }

        public Task<BaseItemDtoQueryResult> GetSeriesEpisodes(Guid seriesId, Guid seasonId)
        {
            return tvShowsClient.GetEpisodesAsync(seriesId, seasonId: seasonId);
        }

        // Custom Rolled
        public Uri GetVideoHLSUri(Guid id, string mediaSourceId)
        {
            return new Uri($"{sdkClientSettings.BaseUrl}/videos/{id}/master.m3u8?api_key={sdkClientSettings.AccessToken}&MediaSourceId={mediaSourceId}");
        }

        public Uri GetImageUri(Guid itemId, ImageType imageType)
        {
            return new Uri($"{sdkClientSettings.BaseUrl}/items/{itemId}/Images/{imageType}?api_key={sdkClientSettings.AccessToken}");
        }

        public Uri GetImageUri(Guid itemId, ImageType imageType, int width, int height)
        {
            return new Uri($"{sdkClientSettings.BaseUrl}/items/{itemId}/Images/{imageType}?api_key={sdkClientSettings.AccessToken}&width={width}&height={height}");
        }
    }
}
