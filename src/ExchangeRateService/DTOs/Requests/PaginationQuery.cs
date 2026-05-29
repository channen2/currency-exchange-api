using System.ComponentModel.DataAnnotations;

namespace ExchangeRateService.DTOs.Requests
{
    public class PaginationQuery
    {
        public const int DefaultPageSize = 20;

        public const int MaxPageSize = 100;

        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; set; } = 1;

        [Range(1, MaxPageSize, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
