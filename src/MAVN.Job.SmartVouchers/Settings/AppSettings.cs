using Lykke.Sdk.Settings;
using MAVN.Job.SmartVouchers.Settings.JobSettings;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.PartnerManagement.Client;
using MAVN.Service.PaymentManagement.Client;

namespace MAVN.Job.SmartVouchers.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public SmartVouchersJobSettings SmartVouchersJob { get; set; }

        public PaymentManagementServiceClientSettings PaymentManagementServiceClient { get; set; }

        public PartnerManagementServiceClientSettings PartnerManagementServiceClient { get; set; }

        public CustomerProfileServiceClientSettings CustomerProfileServiceClient { get; set; }
    }
}
