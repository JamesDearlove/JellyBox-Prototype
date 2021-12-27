using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox.Models
{
    // TODO: This should be observable
    public class BaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? Parent { get; set; }
        public BitmapImage PrimaryImage { get; set; }
        public BitmapImage BackdropImage { get; set; }
        public ImageBlurHashes ImageBlurHashes { get; set; }

        public BaseItem() { }

        public BaseItem(BaseItemDto sdkBaseItem)
        {
            Id = sdkBaseItem.Id;
            Name = sdkBaseItem.Name;
            Parent = sdkBaseItem.ParentId;
            
            // Blur Hashes
        }
    }
}
