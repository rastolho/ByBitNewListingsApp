using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Dtos
{
    public class Listing
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("publishTime")]
        public long PublishTime { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }
    }
}
