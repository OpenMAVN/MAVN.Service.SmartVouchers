using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace MAVN.Service.SmartVouchers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public SmartVouchersSettings SmartVouchersService { get; set; }
    }
}
