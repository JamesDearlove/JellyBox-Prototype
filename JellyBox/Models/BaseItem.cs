﻿using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox.Models
{
    // TODO: This should be observable
    public class BaseItem : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? Parent { get; set; }

        private ImageSource _primaryImage;
        public ImageSource PrimaryImage
        {
            get => _primaryImage;
            set
            {
                _primaryImage = value;
                NotifyPropertyChanged();
            }
        }

        private ImageSource _backdropImage;
        public ImageSource BackdropImage
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

            // TODO: Improve this for being across all hashes
            ImageBlurHashes = new ImageBlurHashes();

            if (sdkBaseItem.ImageBlurHashes.Primary != null)
            {
                ImageBlurHashes.Primary = sdkBaseItem.ImageBlurHashes.Primary.Values.FirstOrDefault();
            }

            if (sdkBaseItem.ImageBlurHashes.Backdrop != null)
            {
                ImageBlurHashes.Backdrop = sdkBaseItem.ImageBlurHashes.Backdrop.Values.FirstOrDefault();
            }
        }

        // TODO: Update this to work across all hashes.
        // TODO: Blur hash aspect ratio scaling.
        public async void CreateBlurImages()
        {
            if (ImageBlurHashes.Primary != null)
            {
                PrimaryImage = await Helpers.GenerateBlurHash(ImageBlurHashes.Primary);
            }
            if (ImageBlurHashes.Backdrop != null)
            {
                BackdropImage = await Helpers.GenerateBlurHash(ImageBlurHashes.Backdrop);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
