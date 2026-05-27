using ExchangeRateService.Data;
using ExchangeRateService.Models;
using ExchangeRateService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateService.Services
{
    public class ExchangeRateIngestionService : IExchangeRateIngestionService
    {
        private readonly AppDbContext _db;
        private readonly ITreasuryExchangeRateService _treasuryService;

        public ExchangeRateIngestionService(
            AppDbContext db,
            ITreasuryExchangeRateService treasuryService
        )
        {
            _db = db;
            _treasuryService = treasuryService;
        }

        public async Task IngestRatesAsync(DateTime fromDate, DateTime toDate)
        {
            var run = new IngestionRun
            {
                Id = Guid.NewGuid(),
                FromDateUtc = fromDate,
                ToDateUtc = toDate,
                StartedAtUtc = DateTime.UtcNow,
            };

            try
            {
                var apiResult = await _treasuryService.GetExchangeRatesAsync(
                    fromDate,
                    toDate,
                    null
                );

                if (!apiResult.IsSuccess)
                {
                    run.Success = false;
                    run.ErrorMessage = apiResult.Error?.Message;

                    _db.IngestionRuns.Add(run);
                    await _db.SaveChangesAsync();
                    return;
                }

                var records = apiResult.Value!.Data;

                foreach (var record in records)
                {
                    if (!DateTime.TryParse(record.RecordDate, out var recordDate))
                    {
                        continue;
                    }

                    if (!DateTime.TryParse(record.EffectiveDate, out var effectiveDate))
                    {
                        continue;
                    }

                    if (!decimal.TryParse(record.ExchangeRate, out var rate))
                    {
                        continue;
                    }

                    var currency = record.CountryCurrencyDescription.ToUpperInvariant();

                    var exists = await _db.ExchangeRates.AnyAsync(x =>
                        x.CurrencyCode == currency
                        && x.EffectiveDate == effectiveDate
                        && x.RecordDate == recordDate
                    );

                    if (exists)
                    {
                        continue;
                    }

                    _db.ExchangeRates.Add(
                        new ExchangeRate
                        {
                            Id = Guid.NewGuid(),
                            CurrencyCode = currency,
                            Rate = rate,
                            EffectiveDate = effectiveDate,
                            RecordDate = recordDate,
                            RetrievedAtUtc = DateTime.UtcNow,
                        }
                    );
                }

                await _db.SaveChangesAsync();

                run.Success = true;
                run.ErrorMessage = null;

                _db.IngestionRuns.Add(run);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                run.Success = false;
                run.ErrorMessage = ex.Message;

                _db.IngestionRuns.Add(run);
                await _db.SaveChangesAsync();
            }
        }
    }
}
