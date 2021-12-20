using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using JellyBox.Pages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JellyBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        ObservableCollection<DisplayItem> UserViewsItems = new ObservableCollection<DisplayItem>();
        ObservableCollection<DisplayItem> LatestShowsItems = new ObservableCollection<DisplayItem>();

        private async void LoadPage()
        {
            UsernameText.Text = Core.JellyfinInstance.LoggedInUser.Name;

            // TODO: Incorporate an image cache.

            // My Media panel
            var userViews = await Core.JellyfinInstance.GetUserViews();
            UserViewsItems.Clear();

            foreach (var userView in userViews)
            {
                var newItem = new DisplayItem();
                newItem.Item = userView;

                var uri = Core.JellyfinInstance.GetImageUri(userView.Id, ImageType.Primary);
                newItem.Image = new BitmapImage(uri);

                UserViewsItems.Add(newItem);
            }

            // TODO: Rework to show Latest views instead.
            var latestMedia = await Core.JellyfinInstance.GetLatestMedia();
            LatestShowsItems.Clear();

            foreach (var item in latestMedia)
            {
                var newItem = new DisplayItem();

                newItem.Item = item;

                var uri = Core.JellyfinInstance.GetImageUri(item.Id, ImageType.Primary);
                newItem.Image = new BitmapImage(uri);

                LatestShowsItems.Add(newItem);
            }
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            LoadPage();
        }

        private void DebugMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestPage));
        }

        private void ReloadPageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Core.JellyfinInstance.ClearSettings();
            Core.SettingManager.Username = null;
            Core.SettingManager.Server = null;
            Core.SettingManager.AccessToken = null;

            Frame.Navigate(typeof(ServerAddressPage));
            Frame.BackStack.Clear();
        }

        private void LatestShowsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as DisplayItem;

            if (item != null)
            {
                Frame.Navigate(typeof(ShowPage), item.Item.Id);
            }
        }
    }
}
