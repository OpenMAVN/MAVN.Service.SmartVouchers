using Autofac;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories;
using MAVN.Service.SmartVouchers.Settings;
using Lykke.SettingsReader;

namespace MAVN.Service.SmartVouchers.Modules
{
    [UsedImplicitly]
    public class DbModule : Module
    {
        private readonly string _connectionString;

        public DbModule(IReloadingManager<AppSettings> appSettings)
        {
            _connectionString = appSettings.CurrentValue.SmartVouchersService.Db.SqlDbConnString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmartVouchersRepository>()
                .As<ISmartVouchersRepository>()
                .SingleInstance();

            builder.RegisterMsSql(
                _connectionString,
                connString => new SmartVouchersContext(connString, false),
                dbConn => new SmartVouchersContext(dbConn));
        }
    }
}
