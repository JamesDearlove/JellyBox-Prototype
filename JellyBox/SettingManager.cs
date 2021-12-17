using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JellyBox
{
    public class SettingManager
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ApplicationDataContainer userDataContainer;


        public SettingManager()
        {
            userDataContainer = localSettings.CreateContainer("UserData", ApplicationDataCreateDisposition.Always);
        }

        public string Server
        {
            get { return (string)userDataContainer.Values["Server"]; }
            set { userDataContainer.Values["Server"] = value; }
        }

        public string Username
        {
            get { return (string)userDataContainer.Values["Username"]; }
            set { userDataContainer.Values["Username"] = value; }
        }

        public string AccessToken
        {
            get { return (string)userDataContainer.Values["AccessToken"]; }
            set { userDataContainer.Values["AccessToken"] = value; }
        }
    }
}
