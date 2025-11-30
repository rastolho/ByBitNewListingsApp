using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Dtos
{
    public class ApiResponse
    {
        public int RetCode { get; set; }
        public string? RetMsg { get; set; }
        public AnnouncementResult? Result { get; set; }
    }
}
