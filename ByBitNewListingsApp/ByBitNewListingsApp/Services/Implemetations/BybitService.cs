using System.Text.Json;
using ByBitNewListingsApp.Configuration;
using ByBitNewListingsApp.Services.Interfaces;
using ByBitNewListingsApp.Dtos;

namespace ByBitNewListingsApp.Services.Implemetations
{
    public class BybitService : IBybitService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;
        private readonly ILogger<BybitService> _logger;
        private const string BaseUrl = "https://api.bybit.com";

        public BybitService(HttpClient httpClient, AppSettings settings, ILogger<BybitService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<List<Listing>> FetchNewsAsync()
        {
            try
            {
                string url = $"{BaseUrl}/v5/announcements/index?locale={_settings.Locale}&limit=20";

                _logger.LogInformation("Fetching news from Bybit API");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString);

                if (apiResponse?.RetCode == 0)
                {
                    _logger.LogInformation("Successfully fetched {Count} news items", apiResponse.Result?.List?.Count ?? 0);
                    return apiResponse.Result?.List ?? new List<Listing>();
                }
                else
                {
                    _logger.LogWarning("API returned error: {Message}", apiResponse?.RetMsg);
                    return new List<Listing>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news from Bybit");
                return new List<Listing>();
            }
        }
    }
}
