using Jellyfin.Sdk;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace JellyBox.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LibraryGridPage : Page
    {

        Models.BaseItem LibraryItem;
        ObservableCollection<Models.BaseItem> LibraryItems { get; set; }

        public LibraryGridPage()
        {
            this.InitializeComponent();

            LibraryItems = new ObservableCollection<Models.BaseItem>();
        }

        private async void LoadPage()
        {
            var items = await Core.JellyfinInstance.GetItems(LibraryItem.Id);
            foreach (var item in items)
            {
                LibraryItems.Add(item);
            }
            foreach (var item in LibraryItems)
            {
                var uri = Core.JellyfinInstance.GetImageUri(item.Id, ImageType.Primary, 300, 450);
                item.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(uri);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LibraryItem = (Models.BaseItem)e.Parameter;
            LoadPage();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.BaseItem;

            if (item != null)
            {
                Frame.Navigate(typeof(MediaDetailsPage), item.Id);
            }
        }
    }
}
