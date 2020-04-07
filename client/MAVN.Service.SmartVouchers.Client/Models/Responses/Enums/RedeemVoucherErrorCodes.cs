namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Redeem voucher error codes
    /// </summary>
    public enum RedeemVoucherErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Voucher not found</summary>
        VoucherNotFound,
        /// <summary>Wrong validation code</summary>
        WrongValidationCode,
        /// <summary>Voucher campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Voucher campaign not active</summary>
        VoucherCampaignNotActive,
    }
}
