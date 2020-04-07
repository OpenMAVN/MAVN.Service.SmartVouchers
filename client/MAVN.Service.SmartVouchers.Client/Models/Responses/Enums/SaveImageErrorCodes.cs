namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Error codes for voucher campaign image saving
    /// </summary>
    public enum SaveImageErrorCodes
    {
        /// <summary>No error code</summary>
        None = 0,
        /// <summary>Voucher campaign not found</summary>
        VoucherCampaignNotFound,
        /// <summary>Invalid file format</summary>
        InvalidFileFormat,
    }
}
