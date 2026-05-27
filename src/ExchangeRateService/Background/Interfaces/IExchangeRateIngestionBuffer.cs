namespace ExchangeRateService.Background.Interfaces
{
    public interface IExchangeRateIngestionBuffer
    {
        ValueTask EnqueueAsync(DateTime fromDate, DateTime toDate);

        ValueTask<(DateTime from, DateTime to)> DequeueAsync(CancellationToken ct);
    }
}
