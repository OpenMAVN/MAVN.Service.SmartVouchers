namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum TransferVoucherError
    {
        None,
        VoucherNotFound,
        NotAnOwner,
        VoucherIsNotInTheCorrectStateToTransfer,
        VoucherCampaignNotFound,
    }
}
