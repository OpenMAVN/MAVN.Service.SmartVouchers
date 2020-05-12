using Autofac;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.SettingsReader;
using MAVN.Job.SmartVouchers.Settings;
using MAVN.Service.SmartVouchers.MsSqlRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MAVN.Job.SmartVouchers
{
    [UsedImplicitly]
    public class Startup
    {
        private IConfigurationRoot _configurationRoot;
        private IReloadingManager<AppSettings> _settingsManager;

        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "SmartVouchersJob API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            (_configurationRoot, _settingsManager) = services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "SmartVouchersJobLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.SmartVouchersJob.Db.LogsConnString;
                };

                options.Extend = (sc, settings) =>
                {
                    sc.AddAutoMapper(typeof(AutoMapperProfile));
                };
            });
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureLykkeContainer(
                _configurationRoot,
                _settingsManager);
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            app.UseLykkeConfiguration(appLifetime, options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }
    }
}
