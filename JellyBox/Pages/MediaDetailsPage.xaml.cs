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
        public ObservableCollection<TvShowEpisode> Episodes { get; set; }
        public ObservableCollection<PersonItem> People { get; set; }

        // TODO: Should these rely on their observable collections?
        public bool SeasonsVisible { get; set; }
        public bool EpisodesVisible { get; set; }
        public bool PeopleVisible { get; set; }

        public BitmapImage BackdropImage { get; set; }
        public BitmapImage PrimaryImage { get; set; }

        public MediaDetailsPage()
        {
            this.InitializeComponent();

            MediaInfo = new BaseMediaItem();
            Seasons = new ObservableCollection<TvShowSeason>();
            Episodes = new ObservableCollection<TvShowEpisode>();
            People = new ObservableCollection<PersonItem>();
        }

        private async void LoadPage()
        {
            MediaInfo = await Core.JellyfinInstance.GetUserLibraryDisplayMediaItem(MediaId);
            RaisePropertyChanged("MediaInfo");

            if (MediaInfo is TvShowSeries)
            {
                SeasonsVisible = true;
                RaisePropertyChanged("SeasonsVisible");
                var seasonsQuery = await Core.JellyfinInstance.GetSeriesSeasons(MediaId);

                Seasons.Clear();
                foreach (var season in seasonsQuery.Items)
                {
                    // TODO: Redo, move to JellyfinInstance
                    var betterSeason = new TvShowSeason(season);
                    Seasons.Add(betterSeason);
                }
            }

            if (MediaInfo is TvShowSeason)
            {
                EpisodesVisible = true;
                RaisePropertyChanged("EpisodesVisible");

                // TODO: This is horrible, this cast should be removed.
                Guid seriesId = (Guid)MediaInfo.ApiItem.ParentId;
                var episodesQuery = await Core.JellyfinInstance.GetSeriesEpisodes(seriesId, MediaId);

                Episodes.Clear();
                foreach (var episode in episodesQuery.Items)
                {
                    var fullEpisode = await Core.JellyfinInstance.GetItem(episode.Id);
                    // TODO: Replace because terrible.
                    var betterEpisode = new TvShowEpisode(fullEpisode);
                    Episodes.Add(betterEpisode);
                }
            }

            PeopleVisible = true;
            RaisePropertyChanged("PeopleVisible");
            foreach (var person in MediaInfo.ApiItem.People)
            {
                var betterPerson = new PersonItem(person);
                People.Add(betterPerson);
            }

            PopulateImages();
        }

        private async void PopulateImages()
        {
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

            foreach (var season in Seasons)
            {
                var imageQuery = Core.JellyfinInstance.GetImageUri(season.Id, ImageType.Primary);
                season.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);
            }
            foreach (var episode in Episodes)
            {
                var imageQuery = Core.JellyfinInstance.GetImageUri(episode.Id, ImageType.Primary);
                episode.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);
            }
            foreach (var person in People)
            {
                var imageQuery = Core.JellyfinInstance.GetImageUri(person.Id, ImageType.Primary);
                person.PrimaryImage = await ImageCache.Instance.GetFromCacheAsync(imageQuery);
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
                Frame.Navigate(typeof(MediaPlayerPage), item);
            }
        }
    }
}
