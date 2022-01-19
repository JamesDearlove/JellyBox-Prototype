using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox.Models
{
    // TODO: Currently not referencing from base class,
    //       should look at very generic base item.
    public class PersonItem : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Initials { get => new String(Name.Split(" ").Select(x => x[0]).ToArray()); }
        public string Role { get; set; }
        public string Type { get; set; }

        private BitmapImage _primaryImage;
        public BitmapImage PrimaryImage
        {
            get => _primaryImage;
            set
            {
                _primaryImage = value;
                NotifyPropertyChanged();
            }
        }
        public ImageBlurHashes ImageBlurHashes { get; set; }

        public PersonItem(BaseItemPerson sdkBaseItem)
        {
            Id = new Guid(sdkBaseItem.Id);
            Name = sdkBaseItem.Name;
            Role = sdkBaseItem.Role;
            Type = sdkBaseItem.Type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
