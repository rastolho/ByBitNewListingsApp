using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Dtos
{
    public class Listing
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long PublishTime { get; set; }
        public string? Url { get; set; }
        public List<string>? Tags { get; set; }
    }
}
