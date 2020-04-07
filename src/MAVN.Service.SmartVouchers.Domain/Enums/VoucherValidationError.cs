namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum VoucherValidationError
    {
        None,
        VoucherCampaignNotFound,
        NoAvailableVouchers,
        VoucherNotFound,
        WrongValidationCode,
    }
}
