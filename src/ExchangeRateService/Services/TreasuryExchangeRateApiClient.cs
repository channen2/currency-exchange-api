using System.Text.Json;
using ExchangeRateService.Common;
using ExchangeRateService.Common.Errors;
using ExchangeRateService.Integrations.Treasury.DTOs;
using ExchangeRateService.Services.Interfaces;

namespace ExchangeRateService.Services
{
    public class TreasuryExchangeRateApiClient(HttpClient httpClient)
        : ITreasuryExchangeRateApiClient
    {
        private readonly HttpClient _httpClient = httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<TreasuryExchangeRateApiResponse>> GetExchangeRatesAsync(
            DateTime fromDate,
            DateTime toDate,
            string? treasuryCurrency = null
        )
        {
            var url =
                "https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange"
                + $"?filter=record_date:gte:{fromDate:yyyy-MM-dd}"
                + $",record_date:lte:{toDate:yyyy-MM-dd}";

            if (!string.IsNullOrWhiteSpace(treasuryCurrency))
            {
                url += $",country_currency_desc:eq:{treasuryCurrency}";
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return Result<TreasuryExchangeRateApiResponse>.Failure(
                    ErrorRegistry.ExternalServiceError,
                    new Dictionary<string, object>
                    {
                        ["statusCode"] = response.StatusCode.ToString(),
                        ["fromDate"] = DateFormats.IsoDate(fromDate),
                        ["toDate"] = DateFormats.IsoDate(toDate),
                    }
                );
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<TreasuryExchangeRateApiResponse>(
                json,
                JsonOptions
            );

            if (data?.Data == null)
            {
                return Result<TreasuryExchangeRateApiResponse>.Failure(
                    ErrorRegistry.ExchangeRateNotFound,
                    new Dictionary<string, object>
                    {
                        ["fromDate"] = DateFormats.IsoDate(fromDate),
                        ["toDate"] = DateFormats.IsoDate(toDate),
                    }
                );
            }

            return Result<TreasuryExchangeRateApiResponse>.Success(data);
        }
    }
}
