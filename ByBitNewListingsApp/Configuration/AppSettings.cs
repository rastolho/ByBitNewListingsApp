using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Configuration
{
    public class AppSettings
    {
        public string? TelegramBotToken { get; set; } 
        public string? TelegramChatId { get; set; }
        public string Locale { get; set; } = "en-US";
        public int CheckIntervalSeconds { get; set; } = 300; // 5 minutes for GitHub Actions
    }
}
