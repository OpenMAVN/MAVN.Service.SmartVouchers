using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using MAVN.Service.SmartVouchers.Services;
using MAVN.Service.SmartVouchers.Settings;
using Lykke.SettingsReader;
using MAVN.Service.SmartVouchers.DomainServices;
using MAVN.Service.SmartVouchers.Domain.Services;
using StackExchange.Redis;

namespace MAVN.Service.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly SmartVouchersSettings _settings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _settings = appSettings.CurrentValue.SmartVouchersService;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<VouchersService>()
                .As<IVouchersService>()
                .WithParameter(TypedParameter.From(_settings.VoucherLockTimeOut))
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<CampaignsService>()
                .As<ICampaignsService>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<FileService>()
                .As<IFileService>()
                .AutoActivate()
                .SingleInstance();

            builder.Register(context =>
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(_settings.Redis.ConnectionString);
                connectionMultiplexer.IncludeDetailInExceptions = false;
                return connectionMultiplexer;
            }).As<IConnectionMultiplexer>().SingleInstance();

            builder.RegisterType<RedisLocksService>()
                .As<IRedisLocksService>()
                .SingleInstance();
        }
    }
}
