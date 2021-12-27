using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace JellyBox.Models
{
    public class TvShowEpisode : BaseItem
    {
        public string Overview { get; set; }
        public string Rating { get; set; }
        public float? CommunityRating { get; set; }
        public string SeasonName { get; set; }
        public DateTime Released { get; set; }

        // TODO: Maybe needs it's own model.
        public string[] Genres { get; set; }

        public Guid? Season { get; set; }
        public Guid? Series { get; set; }

        public TvShowEpisode(BaseItemDto sdkBaseItem) : base(sdkBaseItem)
        {
            Overview = sdkBaseItem.Overview;
            Rating = sdkBaseItem.OfficialRating;
            CommunityRating = sdkBaseItem.CommunityRating;
            SeasonName = sdkBaseItem.SeasonName;
            //Released = sdkBaseItem.AirTime;
            Season = sdkBaseItem.SeasonId;
            Series = sdkBaseItem.SeriesId;
        }
    }
}
