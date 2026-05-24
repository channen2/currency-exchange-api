using System.ComponentModel.DataAnnotations;

namespace ExchangeRateService.DTOs
{
    public class CreateTransactionRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purchase amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase amount must be greater than 0")]
        public decimal PurchaseAmountUsd { get; set; }

        [Required(ErrorMessage = "Transaction date is required")]
        public DateTime? TransactionDate { get; set; }
    }
}
