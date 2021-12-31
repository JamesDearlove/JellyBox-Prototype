using Jellyfin.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyBox.Models
{
    public class BaseMediaItem : BaseItem
    {
        public string Overview { get; set; }
        public string ParentalRating { get; set; }
        public float? CommunityRating { get; set; }
        public DateTimeOffset? PremiereDate { get; set; }
        public virtual string DisplayYear
        {
            get
            {
                if (PremiereDate == null)
                {
                    return null;
                }
                return PremiereDate.Value.Year.ToString();
            }
        }

        // If a subclass has no defined subtitle, default to the DisplayYear property.
        public virtual string SubtitleText
        {
            get
            {
                return DisplayYear;
            }
        }
        public BaseMediaItem() { }
        public BaseMediaItem(BaseItemDto sdkBaseItem) : base(sdkBaseItem)
        {
            Overview = sdkBaseItem.Overview;
            ParentalRating = sdkBaseItem.OfficialRating;
            CommunityRating = sdkBaseItem.CommunityRating;
            PremiereDate = sdkBaseItem.PremiereDate;
        }
    }
}
