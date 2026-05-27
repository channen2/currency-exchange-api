namespace ExchangeRateService.Models
{
    public class IngestionRun
    {
        public Guid Id { get; set; }

        public DateTime FromDateUtc { get; set; }

        public DateTime ToDateUtc { get; set; }

        public DateTime StartedAtUtc { get; set; }

        public DateTime FinishedAtUtc { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
