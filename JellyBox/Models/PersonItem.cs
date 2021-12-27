using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox.Models
{
    // TODO: Currently not referencing from base class,
    //       should look at very generic base item.
    public class PersonItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public BitmapImage PrimaryImage { get; set; }
        public ImageBlurHashes ImageBlurHashes { get; set; }

        public PersonItem(BaseItemPerson sdkBaseItem)
        {
            Id = new Guid(sdkBaseItem.Id);
            Name = sdkBaseItem.Name;
            Role = sdkBaseItem.Role;
            Type = sdkBaseItem.Type;
        }
    }
}
