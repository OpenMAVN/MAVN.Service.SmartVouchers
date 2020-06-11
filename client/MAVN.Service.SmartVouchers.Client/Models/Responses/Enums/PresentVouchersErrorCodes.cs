namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Error codes
    /// </summary>
    public enum PresentVouchersErrorCodes
    {
        /// <summary>
        /// no error
        /// </summary>
        None,
        /// <summary>
        /// Voucher camaaign is missing
        /// </summary>
        VoucherCampaignNotFound,
        /// <summary>
        /// Voucher campaign is not active
        /// </summary>
        VoucherCampaignNotActive,
        /// <summary>
        /// Not enought vouchers in stock
        /// </summary>
        NotEnoughVouchersInStock,
        /// <summary>
        /// Admin user is not the creator of the campaign
        /// </summary>
        IncorrectAdminUser,
        /// <summary>
        /// Could not get redis lock
        /// </summary>
        CouldNotGetLock,
    }
}
