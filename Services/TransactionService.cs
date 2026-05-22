using System.Collections.Concurrent;






using ExchangeRateService.Models;

public class TransactionService
{
    private readonly ConcurrentDictionary<Guid, PurchaseTransaction> _store = new();

    public PurchaseTransaction Create(decimal amount, string currency)
    {
        var tx = new PurchaseTransaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            Currency = currency,
            CreatedAt = DateTime.UtcNow
        };

        _store[tx.Id] = tx;

        return tx;
    }

    public PurchaseTransaction? Get(Guid id)
    {
        _store.TryGetValue(id, out var tx);
        return tx;
    }
}
