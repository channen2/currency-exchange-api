using ExchangeRateService.DTOs;
using ExchangeRateService.Models;
using ExchangeRateService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateService.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<PurchaseTransaction> transactions = await _transactionService.GetAllAsync();

            IEnumerable<TransactionResponse> response = transactions.Select(
                x => new TransactionResponse
                {
                    Id = x.Id,
                    Description = x.Description,
                    PurchaseAmountUsd = x.PurchaseAmountUsd,
                    TransactionDate = x.TransactionDate,
                }
            );

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTransactionRequest request)
        {
            if (request.TransactionDate is null)
            {
                return BadRequest("TransactionDate is required");
            }
            PurchaseTransaction transaction = await _transactionService.Create(
                request.PurchaseAmountUsd,
                request.TransactionDate.Value,
                request.Description
            );

            TransactionResponse response = new()
            {
                Id = transaction.Id,
                Description = transaction.Description,
                PurchaseAmountUsd = transaction.PurchaseAmountUsd,
                TransactionDate = transaction.TransactionDate,
            };

            return Ok(response);
        }
    }
}
