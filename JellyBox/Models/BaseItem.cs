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
    // TODO: This should be observable
    public class BaseItem : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? Parent { get; set; }

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

        private BitmapImage _backdropImage;
        public BitmapImage BackdropImage
        {
            get => _backdropImage;
            set
            {
                _backdropImage = value;
                NotifyPropertyChanged();
            }
        }

        public ImageBlurHashes ImageBlurHashes { get; set; }
        public BaseItemDto ApiItem { get; set; }

        public BaseItem() { }

        public BaseItem(BaseItemDto sdkBaseItem)
        {
            Id = sdkBaseItem.Id;
            Name = sdkBaseItem.Name;
            Parent = sdkBaseItem.ParentId;
            ApiItem = sdkBaseItem;

            // TODO: Blur Hashes
            ImageBlurHashes = new ImageBlurHashes();
            ImageBlurHashes.Primary = sdkBaseItem.ImageBlurHashes.Primary.Values.FirstOrDefault();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
