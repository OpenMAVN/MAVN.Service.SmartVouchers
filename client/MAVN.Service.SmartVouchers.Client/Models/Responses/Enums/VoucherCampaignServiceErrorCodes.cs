namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Voucher campaign error codes
    /// </summary>
    public enum VoucherCampaignServiceErrorCodes
    {
        /// <summary>Empty code</summary>
        None = 0,
        /// <summary>Campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Invalid file format</summary>
        InvalidFileFormat,
    }
}
