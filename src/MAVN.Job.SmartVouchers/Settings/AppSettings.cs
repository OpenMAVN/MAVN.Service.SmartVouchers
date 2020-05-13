using Lykke.Sdk.Settings;
using MAVN.Job.SmartVouchers.Settings.JobSettings;
using MAVN.Service.PaymentManagement.Client;

namespace MAVN.Job.SmartVouchers.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public SmartVouchersJobSettings SmartVouchersJob { get; set; }

        public PaymentManagementServiceClientSettings PaymentManagementServiceClient { get; set; }
    }
}
