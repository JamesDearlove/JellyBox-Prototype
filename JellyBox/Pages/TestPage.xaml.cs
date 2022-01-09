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
using Windows.UI.Xaml.Navigation;

using Jellyfin.Sdk;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.UI.Xaml.Media.Imaging;
using Blurhash.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JellyBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // Don't touch above

        // Amphibia S3 E1
        Guid Example = new Guid("33b4c6d56cc07fcfce7568d5b4130b6d");
        MediaPlayer mediaPlayer = new MediaPlayer();

        private void StreamButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = Core.JellyfinInstance.GetVideoHLSUri(Example, "33b4c6d56cc07fcfce7568d5b4130b6d");

            // Test:
            // mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));

            mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            MediaPlayerThing.SetMediaPlayer(mediaPlayer);

            mediaPlayer.Play();
        }

        private void FlushTest_Click(object sender, RoutedEventArgs e)
        {
            //mediaPlayer.Pause();
            //mediaPlayer.Dispose();
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            //Blurhash.UWP
            var image = Core.BlurhashDecoder.Decode(BlurHashTextBox.Text, 400, 300);

            var softwareImage = new SoftwareBitmapSource();
            await softwareImage.SetBitmapAsync(image);

            BlurHashImage.Source = softwareImage;
        }
    }
}
