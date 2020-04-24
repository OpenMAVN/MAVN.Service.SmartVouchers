using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.SmartVouchers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SmartVouchersSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings Rabbit { get; set; }

        public RedisSettings Redis { set; get; }
        public string LockTimeOut { get; set; }
    }
}
