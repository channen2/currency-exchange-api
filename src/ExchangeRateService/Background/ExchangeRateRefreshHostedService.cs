using ExchangeRateService.Background.Interfaces;
using ExchangeRateService.Logging;

namespace ExchangeRateService.Background
{
    public class ExchangeRateRefreshHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<ExchangeRateRefreshHostedService> logger
    ) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        private readonly ILogger<ExchangeRateRefreshHostedService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var orchestrator =
                scope.ServiceProvider.GetRequiredService<IExchangeRateRefreshOrchestrator>();

            var bootstrapped = await orchestrator.EnsureBootstrapAsync();

            if (bootstrapped)
            {
                // Data is fresh from bootstrap, no need to refresh today
                var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                var delay = tomorrow - DateTime.UtcNow;
                await Task.Delay(delay, stoppingToken);
            }

            // Run immediately on first non-bootstrap start, then every 24h
            while (!stoppingToken.IsCancellationRequested)
            {
                await orchestrator.RefreshRecentAsync();

                LogMessages.RefreshJobExecuted(_logger, DateTime.UtcNow);

                var nextRun = DateTime.UtcNow.Date.AddDays(1);
                var delay = nextRun - DateTime.UtcNow;
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
