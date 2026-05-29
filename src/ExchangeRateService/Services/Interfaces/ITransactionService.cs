using ExchangeRateService.Common;
using ExchangeRateService.Models;

namespace ExchangeRateService.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PurchaseTransaction> CreateAsync(
            decimal amount,
            DateTime transactionDate,
            string description
        );

        Task<PagedResult<PurchaseTransaction>> GetPageAsync(int page, int pageSize);

        Task<PurchaseTransaction?> GetByIdAsync(Guid id);
    }
}
