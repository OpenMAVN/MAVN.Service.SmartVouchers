namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum UpdateCampaignError
    {
        None,
        VoucherCampaignNotFound,
        TotalCountMustBeGreaterThanBoughtVouchersCount,
        CampaignAlreadyStarted,
    }
}
