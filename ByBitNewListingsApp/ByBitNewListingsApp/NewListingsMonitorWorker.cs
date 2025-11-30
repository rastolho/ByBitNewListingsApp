using ByBitNewListingsApp.Configuration;
using ByBitNewListingsApp.Dtos;
using ByBitNewListingsApp.Services.Interfaces;

namespace ByBitNewListingsApp
{
        public class NewListingsMonitorWorker : BackgroundService
        {
            private readonly ILogger<NewListingsMonitorWorker> _logger;
            private readonly IBybitService _bybitService;
            private readonly ITelegramService _telegramService;
            private readonly INewsStorageService _storageService;
            private readonly AppSettings _settings;
            private HashSet<string> _seenNews;
            private bool _isFirstRun = true;

            public NewListingsMonitorWorker(
                ILogger<NewListingsMonitorWorker> logger,
                IBybitService bybitService,
                ITelegramService telegramService,
                INewsStorageService storageService,
                AppSettings settings)
            {
                _logger = logger;
                _bybitService = bybitService;
                _telegramService = telegramService;
                _storageService = storageService;
                _settings = settings;
                _seenNews = _storageService.LoadSeenNews();
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                _logger.LogInformation("Bybit News Monitor Worker starting at: {time}", DateTimeOffset.Now);
                _logger.LogInformation("Check interval: {interval} seconds", _settings.CheckIntervalSeconds);
                _logger.LogInformation("Locale: {locale}", _settings.Locale);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await CheckForNewListingsAsync();

                        if (_isFirstRun)
                        {
                            _isFirstRun = false;
                        }

                        _logger.LogInformation("Next check in {seconds} seconds", _settings.CheckIntervalSeconds);
                        await Task.Delay(TimeSpan.FromSeconds(_settings.CheckIntervalSeconds), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Worker is stopping");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in worker execution");
                        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                    }
                }
            }

            private async Task CheckForNewListingsAsync()
            {
                _logger.LogInformation("Checking for new listings...");

                var news = await _bybitService.FetchNewsAsync();

                if (news.Count == 0)
                {
                    _logger.LogWarning("No news items fetched");
                    return;
                }

                var newListings = new List<Listing>();

                foreach (var item in news)
                {
                    string key = CreateNewsKey(item);

                    if (!_seenNews.Contains(key))
                    {
                        _seenNews.Add(key);
                        newListings.Add(item);
                    }
                }

                if (newListings.Count > 0)
                {
                    _logger.LogInformation("Found {count} new listing(s)", newListings.Count);

                    if (_isFirstRun)
                    {
                        _logger.LogInformation("First run - marking all as seen without notifications");
                    }
                    else
                    {
                        foreach (var item in newListings.OrderByDescending(n => n.PublishTime))
                        {
                            string message = FormatTelegramMessage(item);
                            await _telegramService.SendMessageAsync(message);
                            await Task.Delay(1000); // Rate limiting
                        }
                    }

                    _storageService.SaveSeenNews(_seenNews);
                }
                else
                {
                    _logger.LogInformation("No new listings found");
                }
            }

            private string CreateNewsKey(Listing item)
            {
                return $"{item.Title}_{item.PublishTime}";
            }

            private string FormatTimestamp(long timestampMs)
            {
                try
                {
                    var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampMs).LocalDateTime;
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    return "N/A";
                }
            }

            private string FormatTelegramMessage(Listing item)
            {
                string tags = item.Tags != null && item.Tags.Any()
                    ? string.Join(", ", item.Tags)
                    : "None";

                string description = item.Description ?? "No description";
                if (description.Length > 200)
                {
                    description = description.Substring(0, 200) + "...";
                }

                string message = "🔔 <b>New Bybit Listing!</b>\n\n";
                message += $"<b>{item.Title}</b>\n\n";
                message += $"📅 {FormatTimestamp(item.PublishTime)}\n";
                message += $"🏷️ Tags: {tags}\n\n";
                message += $"{description}\n\n";

                if (!string.IsNullOrEmpty(item.Url))
                {
                    message += $"🔗 <a href=\"{item.Url}\">Read More</a>";
                }

                return message;
            }
        }
    }

