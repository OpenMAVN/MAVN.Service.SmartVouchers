using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using MAVN.Service.PaymentManagement.Client;

namespace MAVN.Service.SmartVouchers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public SmartVouchersSettings SmartVouchersService { get; set; }

        public PaymentManagementServiceClientSettings PaymentManagementServiceClient { get; set; }
    }
}
