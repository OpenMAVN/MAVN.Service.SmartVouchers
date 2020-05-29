namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum RedeemVoucherError
    {
        None = 0,
        VoucherNotFound,
        WrongValidationCode,
        VoucherCampaignNotFound,
        VoucherCampaignNotActive,
        SellerCustomerIsNotALinkedPartner,
        SellerCustomerIsNotTheVoucherIssuer,
    }
}
