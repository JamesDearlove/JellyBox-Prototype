using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JellyBox.Controls
{
    public class CustomMediaTransportControls : MediaTransportControls
    {
        public event EventHandler<EventArgs> BackRequested;

        public string MediaTitle { get; set; }
        public string MediaSubtitle { get; set; }

        public CustomMediaTransportControls()
        {
            this.DefaultStyleKey = typeof(CustomMediaTransportControls);
        }

        protected override void OnApplyTemplate()
        {
            Button backButton = GetTemplateChild("BackButton") as Button;
            backButton.Click += BackButton_Click;

            TextBlock titleTextBlock = GetTemplateChild("TitleValue") as TextBlock;
            titleTextBlock.Text = MediaTitle;

            TextBlock subtitleTextBlock = GetTemplateChild("SubtitleValue") as TextBlock;
            subtitleTextBlock.Text = MediaSubtitle;

            base.OnApplyTemplate();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Raises the back requested event when back is clicked.
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
