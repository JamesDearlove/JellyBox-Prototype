﻿using System;
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

        private async void StreamButton_Click(object sender, RoutedEventArgs e)
        {
            var stream = await Core.JellyfinInstance.GetVideoStream(Example);

            var raStream = stream.Stream;
            var memStream = new MemoryStream();
            raStream.CopyTo(memStream);
            memStream.Position = 0;

            var item = await Core.JellyfinInstance.GetItem(Example);

            mediaPlayer.Source = MediaSource.CreateFromStream(memStream.AsRandomAccessStream(), "video/webm");

            MediaPlayerThing.SetMediaPlayer(mediaPlayer);

            mediaPlayer.Play();
        }
    }
}