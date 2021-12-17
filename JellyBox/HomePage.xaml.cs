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
using Windows.UI.Xaml.Navigation;

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

        private async void LoadPage()
        {
            UsernameText.Text = Core.JellyfinInstance.LoggedInUser.Name;

            var userViews = await Core.JellyfinInstance.GetUserViews();
            ObservableCollection<BaseItemDto> userViewsObservable = new ObservableCollection<BaseItemDto>(userViews);

            //var images = await Core.JellyfinInstance.GetGeneralImages();

            MyMediaGrid.ItemsSource = userViews;
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
    }
}
