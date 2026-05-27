namespace ExchangeRateService.Services.Interfaces
{
    public interface IExchangeRateIngestionService
    {
        Task IngestRatesAsync(DateTime fromDate, DateTime toDate);
    }
}
