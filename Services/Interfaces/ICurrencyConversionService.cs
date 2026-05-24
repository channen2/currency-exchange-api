using ExchangeRateService.Common;
using ExchangeRateService.DTOs;

namespace ExchangeRateService.Services.Interfaces
{
    public interface ICurrencyConversionService
    {
        Task<Result<ConvertedTransactionResponse>> ConvertAsync(
            Guid transactionId,
            string targetCurrency
        );
    }
}
