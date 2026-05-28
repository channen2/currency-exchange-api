using System.Globalization;

namespace ExchangeRateService.Common
{
    public static class DateFormats
    {
        private const string IsoDateFormat = "yyyy-MM-dd";

        public static string IsoDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static DateTime ParseIsoDate(string value)
        {
            return DateTime.ParseExact(
                value,
                IsoDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None
            );
        }
    }
}
