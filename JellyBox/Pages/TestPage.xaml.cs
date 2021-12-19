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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.Storage;

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

        // xadia:8096/videos/33b4c6d5-6cc0-7fcf-ce75-68d5b4130b6d/hls1/main/0.ts?DeviceId=TW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NDsgcnY6OTUuMCkgR2Vja28vMjAxMDAxMDEgRmlyZWZveC85NS4wfDE2MzkxODk4MzQwNjE1&MediaSourceId=33b4c6d56cc07fcfce7568d5b4130b6d&VideoCodec=h264&AudioCodec=aac,mp3&AudioStreamIndex=1&VideoBitrate=139936000&AudioBitrate=64000&PlaySessionId=16e5fde0033c44878d68376570f67f41&api_key=77e9b46863794da09a52b66d06df8d09&SubtitleMethod=Encode&TranscodingMaxAudioChannels=6&RequireAvc=false&Tag=1f0e67a8e2660ff22fd7cc96be73fe07&SegmentContainer=ts&MinSegments=1&BreakOnNonKeyFrames=True&h264-profile=high,main,baseline,constrainedbaseline&h264-level=51&h264-deinterlace=true&TranscodeReasons=VideoCodecNotSupported,AudioCodecNotSupported

        private void StreamButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = Core.JellyfinInstance.GetVideoHLSUri(Example, "33b4c6d56cc07fcfce7568d5b4130b6d");

            //Test:
            //mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));

            mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            MediaPlayerThing.SetMediaPlayer(mediaPlayer);

            mediaPlayer.Play();
        }

        private void FlushTest_Click(object sender, RoutedEventArgs e)
        {
            //mediaPlayer.Pause();
            //mediaPlayer.Dispose();
        }
    }
}
