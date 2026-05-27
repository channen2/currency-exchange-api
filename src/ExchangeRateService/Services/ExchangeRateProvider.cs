using ExchangeRateService.Background.Interfaces;
using ExchangeRateService.Common;
using ExchangeRateService.Common.Errors;
using ExchangeRateService.Data;
using ExchangeRateService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateService.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly AppDbContext _db;
        private readonly ITreasuryExchangeRateService _treasuryService;
        private readonly IExchangeRateIngestionBuffer _ingestionBuffer;

        public ExchangeRateProvider(
            AppDbContext db,
            ITreasuryExchangeRateService treasuryService,
            IExchangeRateIngestionBuffer ingestionBuffer
        )
        {
            _db = db;
            _treasuryService = treasuryService;
            _ingestionBuffer = ingestionBuffer;
        }

        public async Task<Result<decimal>> GetRateAsync(
            string treasuryCurrency,
            DateTime transactionDate
        )
        {
            // 1. DB first (fast path)
            var dbResult = await GetFromDbAsync(treasuryCurrency, transactionDate);

            if (dbResult.IsSuccess)
            {
                return dbResult;
            }

            // 2. Fallback to Treasury API (on-demand)
            var apiResult = await _treasuryService.GetExchangeRatesAsync(
                transactionDate.AddMonths(-6),
                transactionDate,
                treasuryCurrency
            );

            if (!apiResult.IsSuccess || apiResult.Value?.Data == null)
            {
                return Result<decimal>.Failure(
                    ErrorRegistry.ExchangeRateNotFound,
                    new Dictionary<string, object>
                    {
                        ["currency"] = treasuryCurrency,
                        ["transactionDate"] = transactionDate.ToString("yyyy-MM-dd"),
                    }
                );
            }

            var rateResult = ResolveFromApi(apiResult.Value, treasuryCurrency, transactionDate);

            if (!rateResult.IsSuccess)
            {
                return rateResult;
            }

            // Enqueue task to ingest recent rates into DB for future requests
            _ingestionBuffer.EnqueueAsync(transactionDate.AddMonths(-6), transactionDate);

            return rateResult;
        }

        private async Task<Result<decimal>> GetFromDbAsync(
            string treasuryCurrency,
            DateTime transactionDate
        )
        {
            var cutoff = transactionDate.AddMonths(-6);

            var match = await _db
                .ExchangeRates.Where(x =>
                    x.CurrencyCode == treasuryCurrency
                    && x.EffectiveDate <= transactionDate
                    && x.EffectiveDate >= cutoff
                )
                .OrderByDescending(x => x.EffectiveDate)
                .ThenByDescending(x => x.RecordDate)
                .FirstOrDefaultAsync();

            if (match is null)
            {
                return Result<decimal>.Failure(
                    ErrorRegistry.ExchangeRateNotFound,
                    new Dictionary<string, object>
                    {
                        ["transactionDate"] = transactionDate.ToString("yyyy-MM-dd"),
                    }
                );
            }

            return Result<decimal>.Success(match.Rate);
        }

        private static Result<decimal> ResolveFromApi(
            DTOs.Treasury.TreasuryExchangeRateApiResponse api,
            string treasuryCurrency,
            DateTime transactionDate
        )
        {
            var cutoff = transactionDate.AddMonths(-6);

            var bestMatch = api
                .Data.Where(x => x.CountryCurrencyDescription == treasuryCurrency)
                .Select(x => new
                {
                    Record = x,
                    EffectiveDate = DateTime.Parse(x.EffectiveDate),
                    RecordDate = DateTime.Parse(x.RecordDate),
                })
                .Where(x => x.EffectiveDate <= transactionDate)
                .Where(x => x.EffectiveDate >= cutoff)
                .OrderByDescending(x => x.EffectiveDate)
                .ThenByDescending(x => x.RecordDate)
                .FirstOrDefault()
                ?.Record;

            if (bestMatch is null)
            {
                return Result<decimal>.Failure(
                    ErrorRegistry.ExchangeRateNotFound,
                    new Dictionary<string, object>
                    {
                        ["transactionDate"] = transactionDate.ToString("yyyy-MM-dd"),
                    }
                );
            }

            if (!decimal.TryParse(bestMatch.ExchangeRate, out var rate))
            {
                return Result<decimal>.Failure(
                    ErrorRegistry.ExchangeRateParseError,
                    new Dictionary<string, object>
                    {
                        ["exchangeRateValue"] = bestMatch.ExchangeRate,
                    }
                );
            }

            return Result<decimal>.Success(rate);
        }
    }
}
