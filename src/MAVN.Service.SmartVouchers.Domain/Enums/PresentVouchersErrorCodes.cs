namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum PresentVouchersErrorCodes
    {
        None,
        VoucherCampaignNotFound,
        VoucherCampaignNotActive,
        NotEnoughVouchersInStock,
        IncorrectAdminUser,
    }
}
