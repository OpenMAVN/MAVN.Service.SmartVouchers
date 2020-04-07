namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Voucher campaign error codes
    /// </summary>
    public enum VoucherCampaignErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Invalid file format</summary>
        InvalidFileFormat,
        /// <summary>Total count must be greater than bought vouchers count</summary>
        TotalCountMustBeGreaterThanBoughtVouchersCount,
    }
}
