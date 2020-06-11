using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.PartnerManagement.Client;
using MAVN.Service.PaymentManagement.Client;

namespace MAVN.Service.SmartVouchers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public SmartVouchersSettings SmartVouchersService { get; set; }

        public PaymentManagementServiceClientSettings PaymentManagementServiceClient { get; set; }

        public PartnerManagementServiceClientSettings PartnerManagementServiceClient { get; set; }

        public CustomerProfileServiceClientSettings CustomerProfileServiceClient { get; set; }

    }
}
