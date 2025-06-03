using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;
using Sanlam.Banking.Module.Event;
using Sanlam.Banking.Module.Configuration;


namespace Sanlam.Banking.Module.Notification
{
    public class BankingNotificationManager : IBankingNotificationManager
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly ILogger<IBankingNotificationManager> _logger;
        private readonly string _topicArn;

        public BankingNotificationManager(IAmazonSimpleNotificationService snsClient, IOptions<AwsSnsOptions> options, ILogger<BankingNotificationManager> logger)
        {
            _snsClient = snsClient;
            _logger = logger;
            _topicArn = options.Value.BankingNotificationTopicArn;
        }

        public async Task<PublishResponse> PublishEvent(BankingEvent bankingEvent)
        {
            try
            {
                _logger.LogDebug($"Publishing event: {bankingEvent}");

                var message = JsonSerializer.Serialize(bankingEvent);
                var publishResponse = await _snsClient.PublishAsync(new PublishRequest
                {
                    TopicArn = _topicArn,
                    Message = message
                });

                _logger.LogInformation($"Published event: {bankingEvent}");

                return publishResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish event: {bankingEvent}");
                throw;
            }
        }

    }


}
