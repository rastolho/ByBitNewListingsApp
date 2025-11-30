using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Dtos
{
    public class AnnouncementResult
    {
        [JsonPropertyName("list")]
        public List<Listing>? List { get; set; }
    }
}
