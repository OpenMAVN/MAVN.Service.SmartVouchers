using Lykke.SettingsReader.Attributes;

namespace MAVN.Job.SmartVouchers.Settings.JobSettings
{
    public class RabbitMqSettings
    {
        public RabbitMqExchangeSettings Subscribers { get; set; }
        public RabbitMqExchangeSettings Publishers { get; set; }
    }

    public class RabbitMqExchangeSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
