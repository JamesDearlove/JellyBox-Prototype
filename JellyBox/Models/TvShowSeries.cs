using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyBox.Models
{
    public class TvShowSeries : BaseItem
    {
        public string Overview { get; set; }
        public string Rating { get; set; }
        public float CommunityRating { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }

        // Not currently required.
        public string Studio { get; set; }
    }
}
