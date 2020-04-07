using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.SmartVouchers.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [SqlCheck]
        public string SqlDbConnString { get; set; }

        [AzureTableCheck]
        public string CampaignsImageConnString { get; set; }
    }
}
