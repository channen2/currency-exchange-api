namespace ExchangeRateService.Logging
{
    internal static partial class LogMessages
    {
        // -------------------------
        // CACHE
        // -------------------------

        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Debug,
            Message = "Cache hit for {TreasuryCurrency} on {TransactionDate:yyyy-MM-dd}, key={CacheKey}"
        )]
        public static partial void CacheHit(
            ILogger logger,
            string treasuryCurrency,
            DateTime transactionDate,
            string cacheKey
        );

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Debug,
            Message = "Cache miss for {TreasuryCurrency} on {TransactionDate:yyyy-MM-dd}, key={CacheKey}"
        )]
        public static partial void CacheMiss(
            ILogger logger,
            string treasuryCurrency,
            DateTime transactionDate,
            string cacheKey
        );

        // -------------------------
        // DB
        // -------------------------

        [LoggerMessage(
            EventId = 1101,
            Level = LogLevel.Debug,
            Message = "DB hit for {TreasuryCurrency} on {TransactionDate:yyyy-MM-dd}"
        )]
        public static partial void DbHit(
            ILogger logger,
            string treasuryCurrency,
            DateTime transactionDate
        );

        [LoggerMessage(
            EventId = 1102,
            Level = LogLevel.Debug,
            Message = "DB miss for {TreasuryCurrency} on {TransactionDate:yyyy-MM-dd}"
        )]
        public static partial void DbMiss(
            ILogger logger,
            string treasuryCurrency,
            DateTime transactionDate
        );

        // -------------------------
        // API
        // -------------------------

        [LoggerMessage(
            EventId = 1201,
            Level = LogLevel.Warning,
            Message = "Treasury API fallback used for {TreasuryCurrency} ({From} → {To})"
        )]
        public static partial void TreasuryApiFallback(
            ILogger logger,
            string treasuryCurrency,
            DateTime from,
            DateTime to
        );

        [LoggerMessage(
            EventId = 1202,
            Level = LogLevel.Warning,
            Message = "Treasury API call failed for {TreasuryCurrency} from {From} to {To}"
        )]
        public static partial void TreasuryApiFailure(
            ILogger logger,
            string treasuryCurrency,
            DateTime from,
            DateTime to
        );

        [LoggerMessage(
            EventId = 1203,
            Level = LogLevel.Warning,
            Message = "Treasury API parse failure for {TreasuryCurrency}, value={Value}"
        )]
        public static partial void TreasuryApiParseFailure(
            ILogger logger,
            string treasuryCurrency,
            string value
        );

        // -------------------------
        // INGESTION
        // -------------------------

        [LoggerMessage(
            EventId = 1301,
            Level = LogLevel.Information,
            Message = "Ingestion started: {From} → {To}"
        )]
        public static partial void IngestionStarted(ILogger logger, DateTime from, DateTime to);

        [LoggerMessage(
            EventId = 1302,
            Level = LogLevel.Debug,
            Message = "Ingestion record skipped: currency={TreasuryCurrency}, effectiveDate={EffectiveDate}, recordDate={RecordDate}, rawRate={RawExchangeRate}"
        )]
        public static partial void IngestionRecordSkipped(
            ILogger logger,
            string treasuryCurrency,
            string effectiveDate,
            string recordDate,
            string rawExchangeRate
        );

        [LoggerMessage(
            EventId = 1303,
            Level = LogLevel.Information,
            Message = "Ingestion completed: {From} → {To}, inserted={Inserted}"
        )]
        public static partial void IngestionCompleted(
            ILogger logger,
            DateTime from,
            DateTime to,
            int inserted
        );

        [LoggerMessage(
            EventId = 1304,
            Level = LogLevel.Error,
            Message = "Ingestion failed: {From} → {To}"
        )]
        public static partial void IngestionFailed(
            ILogger logger,
            DateTime from,
            DateTime to,
            Exception ex
        );

        // -------------------------
        // BACKGROUND JOBS
        // -------------------------

        [LoggerMessage(
            EventId = 1401,
            Level = LogLevel.Information,
            Message = "Refresh job executed at {UtcNow:O}"
        )]
        public static partial void RefreshJobExecuted(ILogger logger, DateTime utcNow);

        [LoggerMessage(
            EventId = 1402,
            Level = LogLevel.Information,
            Message = "Ingestion worker started at {UtcNow:O}"
        )]
        public static partial void IngestionWorkerStarted(ILogger logger, DateTime utcNow);
    }
}
