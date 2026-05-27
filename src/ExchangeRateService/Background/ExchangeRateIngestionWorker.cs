using ExchangeRateService.Background.Interfaces;
using ExchangeRateService.Services.Interfaces;

namespace ExchangeRateService.Background
{
    public class ExchangeRateIngestionWorker : BackgroundService
    {
        private readonly IExchangeRateIngestionBuffer _buffer;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExchangeRateIngestionWorker(
            IExchangeRateIngestionBuffer buffer,
            IServiceScopeFactory scopeFactory
        )
        {
            _buffer = buffer;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (fromDate, toDate) = await _buffer.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var ingestionService =
                        scope.ServiceProvider.GetRequiredService<IExchangeRateIngestionService>();

                    await ingestionService.IngestRatesAsync(fromDate, toDate);
                }
                catch (Exception ex)
                {
                    // swallow or log later (important: don't kill loop)
                }
            }
        }
    }
}
