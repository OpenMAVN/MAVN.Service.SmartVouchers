namespace MAVN.Service.SmartVouchers.Client.Models.Responses.Enums
{
    /// <summary>
    /// Voucher tarnsfer error codes
    /// </summary>
    public enum TransferVoucherErrorCodes
    {
        /// <summary>No error code</summary>
        None,
        /// <summary>Voucher not found</summary>
        VoucherNotFound,
        /// <summary>Not an owner</summary>
        NotAnOwner,
        /// <summary>Voucher cannot be transferred because it is not in a correct state</summary>
        VoucherIsNotInTheCorrectStateToTransfer,
        /// <summary>
        /// Campaign is missing for smart voucher
        /// </summary>
        VoucherCampaignNotFound,
    }
}
