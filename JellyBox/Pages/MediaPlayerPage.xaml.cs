using JellyBox.Models;
using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class MediaPlayerPage : Page
    {
        BaseMediaItem MediaInfo;
        MediaPlayer MediaPlayer = new MediaPlayer();
        MediaSource MediaSource;

        public MediaPlayerPage()
        {
            this.InitializeComponent();
            MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void LoadMedia()
        {
            var uri = Core.JellyfinInstance.GetVideoHLSUri(MediaInfo.Id, MediaInfo.Id.ToString("N"));
                
            CustomMediaControls.MediaTitle = MediaInfo.PlaybackTitle;
            CustomMediaControls.MediaSubtitle = MediaInfo.PlaybackSubtitle;

            MediaSource = MediaSource.CreateFromUri(uri);
            MediaPlayer.Source = MediaSource;
            MediaPlayerEl.SetMediaPlayer(MediaPlayer);
            MediaPlayer.Play();
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            // Handle returning to previous screen
            //this.Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MediaInfo = (BaseMediaItem)e.Parameter;
            LoadMedia();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            MediaPlayer.Pause();
            MediaPlayer.Source = null;
            MediaSource.Dispose();
            //MediaPlayer.Dispose();
        }
        private void MediaControls_BackRequested(object sender, EventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
