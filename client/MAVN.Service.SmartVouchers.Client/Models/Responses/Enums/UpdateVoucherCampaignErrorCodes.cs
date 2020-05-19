namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Voucher campaign update error codes
    /// </summary>
    public enum UpdateVoucherCampaignErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Total count must be greater than bought vouchers count</summary>
        TotalCountMustBeGreaterThanBoughtVouchersCount,
        /// <summary>Campaign has already started so it cannot be updated</summary>
        CampaignAlreadyStarted,
    }
}
