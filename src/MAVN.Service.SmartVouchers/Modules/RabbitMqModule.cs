using Autofac;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.DomainServices.RabbitSubscribers;
using MAVN.Service.SmartVouchers.Settings;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.SettingsReader;
using MAVN.Service.SmartVouchers.Contract;

namespace MAVN.Service.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string PubExchangeName = "lykke.smart-vouchers.vouchesold";
        private const string SubExchangeName = "REPLACE THIS WITH PROPER EXCHANGE NAME"; // TODO pass proper exchange name

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
                PubExchangeName);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            //builder.RegisterJsonRabbitSubscriber<RabbitSubscriber, object>( // TODO replace object with proper message type
            //    _settings.Subscribers.ConnectionString,
            //    SubExchangeName,
            //    nameof(SmartVouchers).ToLower()); // this could be changed if needed
        }
    }
}
