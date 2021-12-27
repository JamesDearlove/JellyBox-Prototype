using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyBox.Models
{
    public class TvShowSeason : BaseItem
    {
        public string Overview { get; set; }
        public string SeasonName { get; set; }
        public int Season { get; set; }
        public Guid? Series { get; set; }
        public TvShowSeason(BaseItemDto sdkBaseItem) : base(sdkBaseItem)
        {
            Overview = sdkBaseItem.Overview;
            SeasonName = sdkBaseItem.SeasonName;
            //Released = sdkBaseItem.AirTime;
            Series = sdkBaseItem.SeriesId;
        }
    }
}
