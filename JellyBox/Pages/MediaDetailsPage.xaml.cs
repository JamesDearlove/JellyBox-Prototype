using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public sealed partial class MediaDetailsPage : Page, INotifyPropertyChanged
    {

        Guid MediaId;
        public BaseItemDto MediaInfo { get; set; }
        public ObservableCollection<BaseItemDto> Seasons { get; set; }
        public ObservableCollection<BaseItemDto> Episodes { get; set; }

        public MediaDetailsPage()
        {
            this.InitializeComponent();

            MediaInfo = new BaseItemDto();
            Seasons = new ObservableCollection<BaseItemDto>();
            Episodes = new ObservableCollection<BaseItemDto>();
        }

        private async void LoadPage()
        {
            MediaInfo = await Core.JellyfinInstance.GetItem(MediaId);
            RaisePropertyChanged("MediaInfo");

            if (MediaInfo.Type == "Series")
            {
                var seasonsQuery = await Core.JellyfinInstance.GetSeriesSeasons(MediaId);

                Seasons.Clear();
                foreach (var season in seasonsQuery.Items)
                {
                    Seasons.Add(season);
                }
            }

            if (MediaInfo.Type == "Season")
            {
                // TODO: This is horrible, this cast should be removed.
                Guid seriesId = (Guid)MediaInfo.ParentId;
                var episodesQuery = await Core.JellyfinInstance.GetSeriesEpisodes(seriesId, MediaId);

                Episodes.Clear();
                foreach (var episode in episodesQuery.Items)
                {
                    Episodes.Add(episode);
                }
            }

            // TODO: Caching required
            var backgroundImage = Core.JellyfinInstance.GetImageUri(MediaId, ImageType.Backdrop);
            BackdropImage.Source = new BitmapImage(backgroundImage);

            var primaryImage = Core.JellyfinInstance.GetImageUri(MediaId, ImageType.Primary);
            PrimaryImage.Source = new BitmapImage(primaryImage);
        }

        private void SeasonsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as BaseItemDto;

            if (item != null)
            {
                Frame.Navigate(typeof(MediaDetailsPage), item.Id);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MediaId = (Guid)e.Parameter;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EpisodesGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as BaseItemDto;

            if (item != null)
            {
                Frame.Navigate(typeof(MediaPlayerPage), item.Id);
            }
        }
    }
}
