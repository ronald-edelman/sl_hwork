using System.Data;
using System.Net;
using Microsoft.Extensions.Logging;
using Sanlam.Banking.Data.EF;
using Sanlam.Banking.Data.SQL;
using Sanlam.Banking.Module.Contract;
using Sanlam.Banking.Module.Event;
using Sanlam.Banking.Module.Metrics;
using Sanlam.Banking.Module.Notification;
using Sanlam.Banking.Module.Validation;

namespace Sanlam.Banking.Module.Processor
{
    public class BankAccountProcessor : IBankAccountProcessor
    {
        private readonly Data.EF.IAccountRepository _accountRepository;
        private readonly IBankingNotificationManager _notificationManager;
        private readonly ILogger<BankAccountProcessor> _logger;
        private readonly IBankingMetricFactory _bankingMetricFactory;

        public BankAccountProcessor(Data.EF.IAccountRepository accountRepository, IBankingNotificationManager notificationManager, ILogger<BankAccountProcessor> logger, IBankingMetricFactory bankingMetricFactory)
        {
            _accountRepository = accountRepository;
            _notificationManager = notificationManager;
            _logger = logger;
            _bankingMetricFactory = bankingMetricFactory;
        }

        public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request)
        {
            _logger.LogInformation($"Executing withdrawal for account {request.AccountId}");

            var accountId = request.AccountId;
            var amount = request.Amount;

            WithdrawalGuard.AgainstInvalidAccountId(accountId, nameof(accountId));
            WithdrawalGuard.AgainstLessThanZero(amount, nameof(amount));

            //stricter isolation level than repeatable read as correctness matters more especially in a banking application
            using var transaction = await _accountRepository.AsTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var account = await _accountRepository.GetAccountAsync(accountId);
                if (account == null)
                {
                    return WithdrawalResponse.AccountNotFound;
                }

                if (account.Balance < amount)
                {
                    return WithdrawalResponse.InsufficientFunds;
                }

                account.Balance -= amount;

                await _accountRepository.SaveChangesAsync();

                // publish the message inside the transaction boundary - if the message fails the transaction rolls back
                await PublishWithdrawalEventAsync(amount, accountId);

                transaction.Commit();

                return WithdrawalResponse.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during withdrawal");
                _bankingMetricFactory.AddCounter(new Counter()); //for demonstration only to record counters such as exceptions for observability

                await transaction.RollbackAsync();
                return WithdrawalResponse.Error;

            } finally
            {
                _bankingMetricFactory.AddMetric(new Metric()); //for demonstration only to record metrics such as time taken to execute function
            }

        }

        private async Task PublishWithdrawalEventAsync(decimal amount, long accountId)
        {
            var withdrawlEvent = new WithdrawalEvent(amount, accountId, true);
            var publishResponse = await _notificationManager.PublishEvent(withdrawlEvent);

            if (publishResponse.HttpStatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(publishResponse.MessageId))
            {
                //force a failure as the synchronisation message was not delivered, transaction should fail atomically
                throw new Exception("Failed to deliver message");
            }
        }
    }

}
