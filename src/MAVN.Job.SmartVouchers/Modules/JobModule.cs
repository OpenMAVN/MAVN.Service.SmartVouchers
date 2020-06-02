using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.SettingsReader;
using MAVN.Job.SmartVouchers.Services;
using MAVN.Job.SmartVouchers.Settings;

namespace MAVN.Job.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class JobModule : Module
    {
        private readonly AppSettings _settings;
        private readonly IReloadingManager<AppSettings> _settingsManager;

        public JobModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue;
            _settingsManager = settingsManager;
        }

        protected override void Load(ContainerBuilder builder)
        {
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

            builder.RegisterType<ReservedVouchersTimeoutManager>()
                .WithParameter("idlePeriod", _settings.SmartVouchersJob.ReservedVouchersTimeoutJobIdlePeriod)
                .WithParameter("generatePaymentTimeout", _settings.SmartVouchersJob.GeneratePaymentTimeoutPeriod)
                .WithParameter("finishPaymentTimeout", _settings.SmartVouchersJob.FinishPaymentTimeoutPeriod)
                .As<IStartStop>()
                .SingleInstance();

            builder.RegisterType<ExpiredVouchersManager>()
                .WithParameter("idlePeriod", _settings.SmartVouchersJob.ExpiredVouchersJobIdlePeriod)
                .As<IStartStop>()
                .SingleInstance();

            builder.RegisterType<CompletedCampaignsManager>()
                .WithParameter("idlePeriod", _settings.SmartVouchersJob.CompletedCampaignsJobIdlePeriod)
                .As<IStartStop>()
                .SingleInstance();
        }
    }
}
