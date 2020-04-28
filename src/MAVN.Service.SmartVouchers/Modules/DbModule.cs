using Autofac;
using AutoMapper;
using AzureStorage.Blob;
using AzureStorage.Tables;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using Lykke.SettingsReader;
using MAVN.Service.SmartVouchers.AzureRepositories;
using MAVN.Service.SmartVouchers.AzureRepositories.Entities;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories;
using MAVN.Service.SmartVouchers.Settings;

namespace MAVN.Service.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class DbModule : Module
    {
        private const string CampaignsTableName = "CampaignsFiles";

        private readonly string _connectionString;
        private readonly IReloadingManager<string> _rulesImageConnString;

        public DbModule(IReloadingManager<AppSettings> appSettings)
        {
            _connectionString = appSettings.CurrentValue.SmartVouchersService.Db.SqlDbConnString;
            _rulesImageConnString = appSettings.Nested(s => s.SmartVouchersService.Db.CampaignsImageConnString);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CampaignsRepository>()
                .As<ICampaignsRepository>()
                .SingleInstance();

            builder.RegisterType<VouchersRepository>()
                .As<IVouchersRepository>()
                .SingleInstance();

            builder.RegisterType<CampaignContentsRepository>()
                .As<ICampaignContentsRepository>()
                .SingleInstance();

            builder.RegisterType<PaymentRequestsRepository>()
                .As<IPaymentRequestsRepository>()
                .SingleInstance();

            builder.RegisterMsSql(
                _connectionString,
                connString => new SmartVouchersContext(connString, false),
                dbConn => new SmartVouchersContext(dbConn));

            builder.Register(c => new FileRepository(AzureBlobStorage.Create(_rulesImageConnString)))
                .As<IFileRepository>()
                .SingleInstance();

            builder.Register(c =>
                    new FileInfoRepository(
                        AzureTableStorage<FileInfoEntity>.Create(_rulesImageConnString, CampaignsTableName, c.Resolve<ILogFactory>()),
                        c.Resolve<IMapper>()))
                .As<IFileInfoRepository>()
                .SingleInstance();
        }
    }
}
