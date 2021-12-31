using JellyBox.Models;
using Jellyfin.Sdk;
using Microsoft.Toolkit.Uwp.UI;
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
        public BaseMediaItem MediaInfo { get; set; }
        public ObservableCollection<TvShowSeason> Seasons { get; set; }

        public Visibility SeasonsVisible
        {
            get
            {
                return Seasons.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility EpisodesVisible
        {
            get
            {
                return Episodes.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility PeopleVisible
        {
            get
            {
                return People.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ObservableCollection<TvShowEpisode> Episodes { get; set; }
        public ObservableCollection<PersonItem> People { get; set; }
        public BitmapImage BackdropImage { get; set; }
        public BitmapImage PrimaryImage { get; set; }

        public MediaDetailsPage()
        {
            this.InitializeComponent();

            MediaInfo = new Models.BaseMediaItem();
            Seasons = new ObservableCollection<TvShowSeason>();
            Episodes = new ObservableCollection<TvShowEpisode>();
            People = new ObservableCollection<PersonItem>();
        }

        private async void LoadPage()
        {
            MediaInfo = await Core.JellyfinInstance.GetUserLibraryDisplayMediaItem(MediaId);
            RaisePropertyChanged("MediaInfo");

            var backgroundImage = Core.JellyfinInstance.GetImageUri(MediaId, ImageType.Backdrop);
            BackdropImage = await ImageCache.Instance.GetFromCacheAsync(backgroundImage);
            if (BackdropImage == null && MediaInfo.ApiItem.ParentId != null)
            {
                var guid = (Guid)MediaInfo.ApiItem.ParentId;
                backgroundImage = Core.JellyfinInstance.GetImageUri(guid, ImageType.Backdrop);
                BackdropImage = await ImageCache.Instance.GetFromCacheAsync(backgroundImage);
            }
            RaisePropertyChanged("BackdropImage");

            var primaryImage = Core.JellyfinInstance.GetImageUri(MediaId, ImageType.Primary);
            PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(primaryImage);
            RaisePropertyChanged("PrimaryImage");

            if (MediaInfo is TvShowSeries)
            {
                var seasonsQuery = await Core.JellyfinInstance.GetSeriesSeasons(MediaId);

                Seasons.Clear();
                foreach (var season in seasonsQuery.Items)
                {
                    // TODO: Redo
                    var betterSeason = new TvShowSeason(season);

                    var imageQuery = Core.JellyfinInstance.GetImageUri(season.Id, ImageType.Primary);
                    betterSeason.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);

                    Seasons.Add(betterSeason);
                }
            }

            if (MediaInfo is TvShowSeason)
            {
                // TODO: This is horrible, this cast should be removed.
                Guid seriesId = (Guid)MediaInfo.ApiItem.ParentId;
                var episodesQuery = await Core.JellyfinInstance.GetSeriesEpisodes(seriesId, MediaId);

                Episodes.Clear();
                foreach (var episode in episodesQuery.Items)
                {
                    var fullEpisode = await Core.JellyfinInstance.GetItem(episode.Id);
                    // TODO: Replace because terrible.
                    var betterEpisode = new TvShowEpisode(fullEpisode);

                    // TODO: Move out from here
                    var imageQuery = Core.JellyfinInstance.GetImageUri(episode.Id, ImageType.Primary);
                    betterEpisode.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);

                    Episodes.Add(betterEpisode);
                }
            }

            foreach (var person in MediaInfo.ApiItem.People)
            {
                var betterPerson = new PersonItem(person);
                var imageQuery = Core.JellyfinInstance.GetImageUri(betterPerson.Id, ImageType.Primary);
                betterPerson.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);
                People.Add(betterPerson);
            }
        }

        private void SeasonsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TvShowSeason;

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
            var item = e.ClickedItem as TvShowEpisode;

            if (item != null)
            {
                Frame.Navigate(typeof(MediaPlayerPage), item.Id);
            }
        }
    }
}
