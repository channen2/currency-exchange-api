using ExchangeRateService.Common;
using ExchangeRateService.DTOs;
using ExchangeRateService.Models;
using ExchangeRateService.Services;
using ExchangeRateService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateService.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly ICurrencyConversionService _conversionService;

        public TransactionsController(
            TransactionService transactionService,
            ICurrencyConversionService conversionService
        )
        {
            _transactionService = transactionService;
            _conversionService = conversionService;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            PurchaseTransaction? transaction = await _transactionService.GetByIdAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            TransactionResponse response = new()
            {
                Id = transaction.Id,
                Description = transaction.Description,
                PurchaseAmountUsd = transaction.PurchaseAmountUsd,
                TransactionDate = transaction.TransactionDate,
            };

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

        [HttpGet("{id}/convert")]
        public async Task<IActionResult> Convert(Guid id, [FromQuery] string currency)
        {
            Result<ConvertedTransactionResponse> result = await _conversionService.ConvertAsync(
                id,
                currency
            );

            if (!result.IsSuccess)
            {
                return result.Error switch
                {
                    "Transaction not found" => NotFound(result.Error),
                    "Unsupported currency" => BadRequest(result.Error),
                    "No exchange rate available within 6 months" => UnprocessableEntity(
                        result.Error
                    ),
                    _ => BadRequest(result.Error),
                };
            }

            return Ok(result.Value);
        }
    }
}
