namespace ExchangeRateService.Background.Interfaces
{
    public interface IExchangeRateRefreshOrchestrator
    {
        Task<bool> EnsureBootstrapAsync();

        Task RefreshRecentAsync();
    }
}
