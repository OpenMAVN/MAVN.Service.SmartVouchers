using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client
{
    /// <summary>
    /// SmartVouchers client interface.
    /// </summary>
    [PublicAPI]
    public interface ISmartVouchersClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - ISmartVouchersApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IVoucherCampaignsApi Api { get; }
    }
}
