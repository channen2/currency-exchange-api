namespace ExchangeRateService.Models
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }

        public string TreasuryCurrency { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public decimal Rate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime RecordDate { get; set; }

        public DateTime RetrievedAtUtc { get; set; }
    }
}
