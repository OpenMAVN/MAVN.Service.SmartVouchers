namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum BuyVoucherError
    {
        None = 0,
        VoucherCampaignNotFound,
        VoucherCampaignNotActive,
        NoAvailableVouchers,
    }
}
