using System;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface IVouchersService
    {
        Task<BuyVoucherError> BuyVoucherAsync(Guid voucherCampaignId, Guid ownerId);
        Task<RedeemVoucherError> RedeemVoucherAsync(string voucherShortCode, string validationCode);
        Task<TransferVoucherError> TransferVoucherAsync(
            string voucherShortCode,
            Guid oldOwnerId,
            Guid newOwnerId);
        Task<VoucherWithValidation> GetByShortCodeAsync(string voucherShortCode);
        Task<VouchersPage> GetCustomerVouchersAsync(Guid customerId, PageInfo pageInfo);
        Task<VouchersPage> GetCampaignVouchersAsync(Guid campaignId, PageInfo pageInfo);
    }
}
