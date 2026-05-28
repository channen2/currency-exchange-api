using ExchangeRateService.Common;
using ExchangeRateService.Integrations.Treasury.DTOs;

namespace ExchangeRateService.Services.Interfaces
{
    public interface ITreasuryExchangeRateApiClient
    {
        Task<Result<TreasuryExchangeRateApiResponse>> GetExchangeRatesAsync(
            DateTime fromDate,
            DateTime toDate,
            string? treasuryCurrency
        );
    }
}
