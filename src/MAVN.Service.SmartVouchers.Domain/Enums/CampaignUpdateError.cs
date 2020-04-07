namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum CampaignUpdateError
    {
        None,
        VoucherCampaignNotFound,
        TotalCountMustBeGreaterThanBoughtVouchersCount,
    }
}
