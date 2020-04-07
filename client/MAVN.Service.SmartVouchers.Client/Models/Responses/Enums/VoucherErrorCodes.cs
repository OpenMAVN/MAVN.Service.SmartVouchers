namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Voucher error codes
    /// </summary>
    public enum VoucherErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Voucher campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>No available vouchers</summary>
        NoAvailableVouchers,
        /// <summary>Voucher not found</summary>
        VoucherNotFound,
        /// <summary>Wrong validation code</summary>
        WrongValidationCode,
        /// <summary>Not an owner</summary>
        NotAnOwner,
        /// <summary>Voucher is already used</summary>
        VoucherIsUsed,
    }
}
