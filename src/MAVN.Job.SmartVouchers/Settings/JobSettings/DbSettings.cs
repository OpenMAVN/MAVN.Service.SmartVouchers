using Lykke.SettingsReader.Attributes;

namespace MAVN.Job.SmartVouchers.Settings.JobSettings
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
