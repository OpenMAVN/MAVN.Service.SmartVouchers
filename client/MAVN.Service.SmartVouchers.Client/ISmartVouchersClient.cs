using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client
{
    /// <summary>
    /// SmartVouchers client interface.
    /// </summary>
    [PublicAPI]
    public interface ISmartVouchersClient
    {
        /// <summary>Voucher campaigns Api interface</summary>
        IVoucherCampaignsApi CampaignsApi { get; }

        /// <summary>Vouchers Api interface</summary>
        ISmartVouchersApi VouchersApi { get; }
    }
}
