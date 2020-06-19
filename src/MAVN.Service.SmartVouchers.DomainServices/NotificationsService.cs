using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Common;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystem.SubscriberContract;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    public class NotificationsService : INotificationsService
    {
        private readonly IRabbitPublisher<PushNotificationEvent> _pushNotificationPublisher;
        private readonly string _voucherRedemptionSucceededTemplateId;

        public NotificationsService(
            IRabbitPublisher<PushNotificationEvent> pushNotificationPublisher,
            string voucherRedemptionSucceededTemplateId)
        {
            _pushNotificationPublisher = pushNotificationPublisher;
            _voucherRedemptionSucceededTemplateId = voucherRedemptionSucceededTemplateId;
        }

        public Task PublishVoucherSuccessfullyRedeemed(string customerId, string partnerName, string voucherShortCode)
        {
            return _pushNotificationPublisher.PublishAsync(new PushNotificationEvent
            {
                CustomerId = customerId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                MessageTemplateId = _voucherRedemptionSucceededTemplateId,
                TemplateParameters = new Dictionary<string, string> { { "PartnerName", partnerName }, { "VoucherShortCode", voucherShortCode } },
                CustomPayload = new Dictionary<string, string> { { "route", "voucher-usage-success" } }
            });
        }
    }
}
