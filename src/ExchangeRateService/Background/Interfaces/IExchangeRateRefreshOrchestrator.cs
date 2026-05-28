namespace ExchangeRateService.Background.Interfaces
{
    public interface IExchangeRateRefreshOrchestrator
    {
        Task EnsureBootstrapAsync();

        Task RefreshRecentAsync();
    }
}
