# ByBit New Listings Monitor

A .NET console application that monitors ByBit for new cryptocurrency listings and sends real-time notifications via Telegram.

## Features

- 🚀 **Real-time Monitoring**: Continuously checks ByBit for new listings
- 📱 **Telegram Notifications**: Instant alerts for new cryptocurrencies
- 🐳 **Docker Support**: Easy deployment with Docker containers
- ⚙️ **Configurable**: Customize check intervals and notification settings
- 🔄 **Auto-restart**: Runs continuously with automatic restart on failure
- 📊 **Data Persistence**: Stores data in a persistent volume

## Prerequisites

- .NET 9.0 or higher
- Docker & Docker Compose (for containerized deployment)
- Telegram Bot Token and Chat ID
- ByBit API access (public endpoints)

## Quick Start

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/rastolho/ByBitNewListingsApp.git
   cd ByBitNewListingsApp
   ```

2. **Configure environment variables**
   ```bash
   export TELEGRAM_BOT_TOKEN=your_bot_token_here
   export TELEGRAM_CHAT_ID=your_chat_id_here
   export CHECK_INTERVAL_SECONDS=60
   ```

3. **Build and run**
   ```bash
   dotnet build
   dotnet run --project ByBitNewListingsApp
   ```

### Docker Deployment

1. **Build the Docker image**
   ```bash
   docker build -t bybit-monitor-image .
   ```

2. **Run the container**
   ```bash
   docker run -d --restart unless-stopped \
     --name bybit-monitor-container \
     -v bybit-data:/app/data \
     -e TELEGRAM_BOT_TOKEN=your_bot_token \
     -e TELEGRAM_CHAT_ID=your_chat_id \
     -e CHECK_INTERVAL_SECONDS=60 \
     bybit-monitor-image
   ```

### VPS Deployment with GitHub Actions

This project includes automated CI/CD with GitHub Actions that:
- Builds the .NET application
- Creates a Docker image
- Deploys to your VPS via SSH
- Runs the container with auto-restart

**Setup Instructions:**

1. **Create GitHub Secrets** in your repository settings:
   - `SSH_PRIVATE_KEY`: Your VPS SSH private key
   - `SSH_HOST`: Your VPS IP address
   - `SSH_USER`: SSH username (e.g., `deployer`)
   - `TELEGRAM_BOT_TOKEN`: Your Telegram bot token
   - `TELEGRAM_CHAT_ID`: Your Telegram chat ID

2. **Automated Deployment** triggers on:
   - Push to `main` branch
   - Manual workflow trigger
   - Hourly schedule (every hour)

## Configuration

### Environment Variables

| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `TELEGRAM_BOT_TOKEN` | Telegram bot authentication token | N/A | Yes |
| `TELEGRAM_CHAT_ID` | Telegram chat ID for notifications | N/A | Yes |
| `CHECK_INTERVAL_SECONDS` | Interval between ByBit checks (seconds) | 60 | No |
| `LOCALE` | Application locale | en-US | No |

### Telegram Setup

1. Create a bot with [@BotFather](https://t.me/botfather) on Telegram
2. Get your bot token
3. Start a conversation with your bot
4. Get your chat ID from [@userinfobot](https://t.me/userinfobot)

## Project Structure

```
ByBitNewListingsApp/
├── ByBitNewListingsApp/
│   ├── Program.cs              # Application entry point
│   ├── ByBitNewListingsApp.csproj
│   └── [services & models]
├── Dockerfile                   # Docker image definition
├── .github/
│   └── workflows/
│       └── deploy.yml           # GitHub Actions CI/CD
└── README.md
```

## Monitoring & Troubleshooting

### View Application Logs

**Using Docker:**
```bash
# Real-time logs
docker logs bybit-monitor-container -f

# Last 50 lines
docker logs bybit-monitor-container --tail 50

# With timestamps
docker logs bybit-monitor-container -t
```

### Check Container Status

```bash
# List running containers
docker ps

# Inspect container
docker inspect bybit-monitor-container

# Check container resource usage
docker stats bybit-monitor-container
```

### Common Issues

**Container exits immediately:**
- Check environment variables are set correctly
- View logs: `docker logs bybit-monitor-container`
- Ensure Telegram credentials are valid

**No notifications received:**
- Verify `TELEGRAM_BOT_TOKEN` and `TELEGRAM_CHAT_ID` are correct
- Check if bot has permission to send messages
- Review application logs for errors

**High CPU/Memory usage:**
- Increase `CHECK_INTERVAL_SECONDS` to reduce API calls
- Check ByBit API limits

## CI/CD Pipeline

The GitHub Actions workflow:

1. **Checkout** - Pulls latest code from repository
2. **SSH Setup** - Configures secure VPS connection
3. **Build** - Compiles .NET 9.0 application
4. **Publish** - Creates release build
5. **Transfer** - Uploads files to VPS via SCP
6. **Deploy** - Builds Docker image and starts container
7. **Verify** - Shows container logs for confirmation

The workflow runs:
- On every push to `main` branch
- On manual trigger via GitHub Actions UI
- Every hour on schedule

## Performance Tips

- Adjust `CHECK_INTERVAL_SECONDS` based on your needs (lower = more frequent checks)
- Monitor VPS resource usage with `docker stats`
- Use persistent volumes to track already-notified listings
- Consider rate limiting from ByBit API

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the MIT License.

## Support

For issues, questions, or suggestions:
- Open a GitHub Issue
- Check existing issues for solutions
- Review application logs for error messages

## Disclaimer

This application monitors public ByBit APIs. Ensure you comply with ByBit's Terms of Service and rate limiting policies. The developer is not responsible for any issues arising from API usage.

---

**Happy monitoring! 🚀**
