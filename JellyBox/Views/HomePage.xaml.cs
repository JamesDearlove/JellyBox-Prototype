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
using Microsoft.Toolkit.Uwp.UI;

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

        // TODO: Move to their respective items.



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

            Frame.Navigate(typeof(ServerLoginPage));
            Frame.BackStack.Clear();
        }


        private void MyMediaGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.BaseItem;

            if (item != null)
            {
                Frame.Navigate(typeof(LibraryGridPage), item);
            }
        }

        private void ContinueWatchingGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.BaseMediaItem;

            if (item != null)
            {
                var playbackItem = new Models.PlaybackItem(item, true);

                Frame.Navigate(typeof(MediaPlayerPage), playbackItem);
            }
        }

        private void LatestShowsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.BaseItem;

            if (item != null)
            {
                Frame.Navigate(typeof(MediaDetailsPage), item.Id);
            }
        }

    }
}
