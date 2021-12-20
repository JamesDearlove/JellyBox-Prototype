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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JellyBox.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowPage : Page
    {

        Guid ShowId;
        BaseItemDto showInfo;

        public ShowPage()
        {
            this.InitializeComponent();
        }

        private async void LoadPage()
        {
            // TODO: This needs to be moved into a DataContext
            showInfo = await Core.JellyfinInstance.GetItem(ShowId);

            ShowNameText.Text = showInfo.Name;
            RatingText.Text = showInfo.OfficialRating;
            OverviewText.Text = showInfo.Overview;
            CommunityRatingText.Text = showInfo.CommunityRating.ToString();

            var backgroundImage = Core.JellyfinInstance.GetImageUri(ShowId, ImageType.Backdrop);
            BackdropImage.Source = new BitmapImage(backgroundImage);

            var primaryImage = Core.JellyfinInstance.GetImageUri(ShowId, ImageType.Primary);
            PrimaryImage.Source = new BitmapImage(primaryImage);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowId = (Guid)e.Parameter;
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            LoadPage();
        }

        
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
