namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Processing voucher error codes
    /// </summary>
    public enum ProcessingVoucherErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Voucher campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Voucher campaign not active</summary>
        VoucherCampaignNotActive,
        /// <summary>No available vouchers</summary>
        NoAvailableVouchers,
        /// <summary>Voucher not found</summary>
        VoucherNotFound,
    }
}
