using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox
{
    public static class Core
    {
        public static JellyfinInstance JellyfinInstance = new JellyfinInstance();
        public static SettingManager SettingManager = new SettingManager();
        public static Blurhash.UWP.Decoder BlurhashDecoder = new Blurhash.UWP.Decoder();
    }
}
