using Amazon.SimpleNotificationService.Model;
using Sanlam.Banking.Module.Event;

namespace Sanlam.Banking.Module.Notification
{
    public interface IBankingNotificationManager
    {

        public Task<PublishResponse> PublishEvent(BankingEvent bankingEvent);

    }
}
