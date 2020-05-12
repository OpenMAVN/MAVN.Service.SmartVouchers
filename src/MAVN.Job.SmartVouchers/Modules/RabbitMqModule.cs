using Autofac;
using JetBrains.Annotations;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.SettingsReader;
using MAVN.Job.SmartVouchers.Settings;
using MAVN.Job.SmartVouchers.Settings.JobSettings;
using MAVN.Service.PaymentManagement.Contract;
using MAVN.Service.SmartVouchers.Contract;
using MAVN.Service.SmartVouchers.DomainServices.RabbitSubscribers;

namespace MAVN.Job.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string PubExchangeName = "lykke.smart-vouchers.vouchersold";
        private const string SubExchangeName = "lykke.payment.completed"; // TODO pass proper exchange name

        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue.SmartVouchersJob.Rabbit;
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
                PubExchangeName);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitSubscriber<RabbitSubscriber, PaymentCompletedEvent>(
                _settings.Subscribers.ConnectionString,
                SubExchangeName,
                nameof(Service.SmartVouchers).ToLower()); // this could be changed if needed
        }
    }
}
