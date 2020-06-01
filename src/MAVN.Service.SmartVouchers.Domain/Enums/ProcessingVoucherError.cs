namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum ProcessingVoucherError
    {
        None = 0,
        VoucherCampaignNotFound,
        VoucherCampaignNotActive,
        NoAvailableVouchers,
        VoucherNotFound,
        InvalidPartnerPaymentConfiguration,
        CustomerHaveAnotherReservedVoucher,
    }
}
