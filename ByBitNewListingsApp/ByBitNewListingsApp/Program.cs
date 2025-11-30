using ByBitNewListingsApp;
using ByBitNewListingsApp.Configuration;
using ByBitNewListingsApp.Services.Implemetations;
using ByBitNewListingsApp.Services.Interfaces;

var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Load settings from environment variables
                    var settings = new AppSettings
                    {
                        TelegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN") ?? "",
                        TelegramChatId = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID") ?? "",
                        Locale = Environment.GetEnvironmentVariable("LOCALE") ?? "en-US",
                        CheckIntervalSeconds = int.TryParse(
                            Environment.GetEnvironmentVariable("CHECK_INTERVAL_SECONDS"),
                            out int interval) ? interval : 300
                    };

                    if (string.IsNullOrWhiteSpace(settings.TelegramBotToken))
                    {
                        throw new InvalidOperationException("TELEGRAM_BOT_TOKEN environment variable is required");
                    }

                    if (string.IsNullOrWhiteSpace(settings.TelegramChatId))
                    {
                        throw new InvalidOperationException("TELEGRAM_CHAT_ID environment variable is required");
                    }

                    services.AddSingleton(settings);
                    services.AddHttpClient<ITelegramService, TelegramService>();
                    services.AddHttpClient<IBybitService, BybitService>();
                    services.AddSingleton<INewsStorageService, NewListingsStorageService>();
                    services.AddHostedService<NewListingsMonitorWorker>();
                })
                .Build();

await host.RunAsync();
