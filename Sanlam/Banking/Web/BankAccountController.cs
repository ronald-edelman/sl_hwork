using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sanlam.Banking.Module.Contract;
using Sanlam.Banking.Module.Metrics;
using Sanlam.Banking.Module.Processor;

namespace Sanlam.Banking.Web
{
    [ApiController]
    [Route("bank")]

    /// <summary>
    /// Restful controller to manage banking transactions such as Withdraw from account
    /// </summary>
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountProcessor _bankAccountProcessor;
        private readonly ILogger<BankAccountController> _logger;
        private readonly IBankingMetricFactory _bankingMetricFactory;

        public BankAccountController(IBankAccountProcessor bankAccountProcessor, ILogger<BankAccountController> logger, IBankingMetricFactory bankingMetricFactory)
        {
            _bankAccountProcessor = bankAccountProcessor;
            _logger = logger;
            _bankingMetricFactory = bankingMetricFactory;  
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequest request)
        {
            try
            {
                _logger.LogInformation($"Executing withdrawal for account {request.AccountId}");

                var withdrawalResponse = await _bankAccountProcessor.WithdrawAsync(request);

                switch (withdrawalResponse.UpdateResult)
                {
                    case BalanceUpdateResult.Success:
                        return Ok("Withdrawal successful");

                    case BalanceUpdateResult.InsufficientFunds:
                        return BadRequest("Insufficient funds for withdrawal");

                    case BalanceUpdateResult.AccountNotFound:
                        return Unauthorized("Account not found or unauthorized access");

                    case BalanceUpdateResult.Error:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Withdrawal failed due to a system error");

                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during withdrawal");
                _bankingMetricFactory.AddCounter(new Counter()); //for demonstration only to record counters such as exceptions on controller methods

                return StatusCode(StatusCodes.Status500InternalServerError, "Withdrawal failed due to an unexpected error");
            }

        }
    }

}
