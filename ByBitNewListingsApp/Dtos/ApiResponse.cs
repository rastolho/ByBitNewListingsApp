using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Dtos
{
    public class ApiResponse
    {
        [JsonPropertyName("retCode")]
        public int RetCode { get; set; }
        [JsonPropertyName("retMsg")]
        public string? RetMsg { get; set; }
        [JsonPropertyName("result")]
        public AnnouncementResult? Result { get; set; }
    }
}
