using Autofac;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.DomainServices.RabbitSubscribers;
using MAVN.Service.SmartVouchers.Settings;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.SettingsReader;
using MAVN.Service.NotificationSystem.SubscriberContract;
using MAVN.Service.PaymentManagement.Contract;
using MAVN.Service.SmartVouchers.Contract;

namespace MAVN.Service.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string PubVoucherSoldExchangeName = "lykke.smart-vouchers.vouchersold";
        private const string PubVoucherUsedExchangeName = "lykke.smart-vouchers.voucherused";
        private const string PubVoucherTransferredExchangeName = "lykke.smart-vouchers.vouchertransferred";
        private const string PubPushNotificationExchangeName = "notificationsystem.command.pushnotification";
        private const string SubExchangeName = "lykke.payment.completed"; // TODO pass proper exchange name

        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue.SmartVouchersService.Rabbit;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them

            RegisterRabbitMqPublishers(builder);

            RegisterRabbitMqSubscribers(builder);
        }

        // registered publishers could be esolved by IRabbitPublisher<TMessage> interface
        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitPublisher<SmartVoucherSoldEvent>(
                _settings.Publishers.ConnectionString,
                PubVoucherSoldExchangeName);
            builder.RegisterJsonRabbitPublisher<SmartVoucherUsedEvent>(
                _settings.Publishers.ConnectionString,
                PubVoucherUsedExchangeName);
            builder.RegisterJsonRabbitPublisher<SmartVoucherTransferredEvent>(
                _settings.Publishers.ConnectionString,
                PubVoucherTransferredExchangeName);
            builder.RegisterJsonRabbitPublisher<PushNotificationEvent>(
                _settings.Publishers.ConnectionString,
                PubPushNotificationExchangeName);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitSubscriber<RabbitSubscriber, PaymentCompletedEvent>(
                _settings.Subscribers.ConnectionString,
                SubExchangeName,
                nameof(SmartVouchers).ToLower()); // this could be changed if needed
        }
    }
}
