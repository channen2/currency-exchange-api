using ExchangeRateService.Common;
using ExchangeRateService.Data;
using ExchangeRateService.Models;
using ExchangeRateService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateService.Services
{
    public class TransactionService(AppDbContext db) : ITransactionService
    {
        private readonly AppDbContext _db = db;

        public async Task<PurchaseTransaction> CreateAsync(
            decimal amount,
            DateTime transactionDate,
            string description
        )
        {
            PurchaseTransaction transaction = new PurchaseTransaction
            {
                Id = Guid.NewGuid(),
                Description = description,
                PurchaseAmountUsd = amount,
                TransactionDate = transactionDate,
                CreatedAt = DateTime.UtcNow,
            };

            _db.PurchaseTransactions.Add(transaction);
            await _db.SaveChangesAsync();

            return transaction;
        }

        public async Task<PagedResult<PurchaseTransaction>> GetPageAsync(int page, int pageSize)
        {
            IQueryable<PurchaseTransaction> query = _db.PurchaseTransactions.OrderByDescending(x =>
                x.TransactionDate
            );

            var totalCount = await query.CountAsync();

            List<PurchaseTransaction> items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PurchaseTransaction>(items, totalCount);
        }

        public async Task<PurchaseTransaction?> GetByIdAsync(Guid id)
        {
            return await _db.PurchaseTransactions.FindAsync(id);
        }
    }
}
