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
        Guid MediaId;
        MediaPlayer MediaPlayer = new MediaPlayer();

        MediaSource MediaSource;

        public MediaPlayerPage()
        {
            this.InitializeComponent();
        }

        private void LoadMedia()
        {
            var uri = Core.JellyfinInstance.GetVideoHLSUri(MediaId, MediaId.ToString("N"));

            // Test:
            // mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));

            MediaSource = MediaSource.CreateFromUri(uri);
            MediaPlayer.Source = MediaSource;
            MediaPlayerEl.SetMediaPlayer(MediaPlayer);

            MediaPlayer.Play();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MediaId = (Guid)e.Parameter;
            LoadMedia();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            MediaPlayer.Pause();
            //MediaPlayerEl.SetMediaPlayer(null);
            MediaPlayer.Source = null;
            MediaSource.Dispose();
            //MediaPlayer.Dispose();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
