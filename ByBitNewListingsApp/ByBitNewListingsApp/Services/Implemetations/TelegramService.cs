using ByBitNewListingsApp.Configuration;
using ByBitNewListingsApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Services.Implemetations
{
    public class TelegramService : ITelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(HttpClient httpClient, AppSettings settings, ILogger<TelegramService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            try
            {
                string url = $"https://api.telegram.org/bot{_settings.TelegramBotToken}/sendMessage";

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("chat_id", _settings.TelegramChatId),
                    new KeyValuePair<string, string>("text", message),
                    new KeyValuePair<string, string>("parse_mode", "HTML")
                });

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Telegram message sent successfully");
                    return true;
                }
                else
                {
                    _logger.LogWarning("Failed to send Telegram message: {StatusCode}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Telegram message");
                return false;
            }
        }
    }
}
