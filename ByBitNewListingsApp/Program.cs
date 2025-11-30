using ByBitNewListingsApp;
using ByBitNewListingsApp.Configuration;
using ByBitNewListingsApp.Services.Implemetations;
using ByBitNewListingsApp.Services.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Load settings from configuration (includes User Secrets in Development)
        var settings = new AppSettings
        {
            TelegramBotToken = hostContext.Configuration["TELEGRAM_BOT_TOKEN"] ?? "",
            TelegramChatId = hostContext.Configuration["TELEGRAM_CHAT_ID"] ?? "",
            Locale = hostContext.Configuration["LOCALE"] ?? "en-US",
            CheckIntervalSeconds = int.TryParse(
                hostContext.Configuration["CHECK_INTERVAL_SECONDS"],
                out int interval) ? interval : 300
        };

        if (string.IsNullOrWhiteSpace(settings.TelegramBotToken))
        {
            throw new InvalidOperationException("TELEGRAM_BOT_TOKEN is required");
        }
        if (string.IsNullOrWhiteSpace(settings.TelegramChatId))
        {
            throw new InvalidOperationException("TELEGRAM_CHAT_ID is required");
        }

        services.AddSingleton(settings);
        services.AddHttpClient<ITelegramService, TelegramService>();
        services.AddHttpClient<IBybitService, BybitService>();
        services.AddSingleton<INewsStorageService, NewListingsStorageService>();
        services.AddHostedService<NewListingsMonitorWorker>();
    })
    .Build();

await host.RunAsync();