namespace ExchangeRateService.DTOs.Responses
{
    public record SupportedCurrencyResponse(
        string CurrencyCode,
        string TreasuryCurrencyDescription
    );
}
