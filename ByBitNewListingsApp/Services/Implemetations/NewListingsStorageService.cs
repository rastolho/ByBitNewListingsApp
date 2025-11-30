using ByBitNewListingsApp.Services.Interfaces;
using System.IO; 
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ByBitNewListingsApp.Services.Implemetations
{
    public class NewListingsStorageService : INewsStorageService
    {

        private const string DataDirectory = "/app/data";
        private static readonly string SeenNewsFilePath = Path.Combine(DataDirectory, "seen_news.json");

        private readonly ILogger<NewListingsStorageService> _logger;

        public NewListingsStorageService(ILogger<NewListingsStorageService> logger)
        {
            _logger = logger;

            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
                _logger.LogInformation("Created persistent data directory: {Dir}", DataDirectory);
            }
        }

        public HashSet<string> LoadSeenNews()
        {
            try
            {
                if (File.Exists(SeenNewsFilePath))
                {
                    string json = File.ReadAllText(SeenNewsFilePath);
                    var list = JsonSerializer.Deserialize<List<string>>(json);
                    _logger.LogInformation("Loaded {Count} seen news items from {Path}", list?.Count ?? 0, SeenNewsFilePath);
                    return new HashSet<string>(list ?? new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not load seen news file from {Path}", SeenNewsFilePath);
            }
            return new HashSet<string>();
        }

        public void SaveSeenNews(HashSet<string> seenNews)
        {
            try
            {
                var list = seenNews.ToList();
                string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(SeenNewsFilePath, json);
                _logger.LogInformation("Saved {Count} seen news items to {Path}", list.Count, SeenNewsFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not save seen news file to {Path}", SeenNewsFilePath);
            }
        }
    }
}