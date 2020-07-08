using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface IVouchersService
    {
        Task<ProcessingVoucherError> ProcessPaymentRequestAsync(Guid paymentRequestId);
        Task<VoucherReservationResult> ReserveVoucherAsync(Guid voucherCampaignId, Guid ownerId);
        Task<ProcessingVoucherError> CancelVoucherReservationAsync(string shortCode);
        Task<RedeemVoucherError> RedeemVoucherAsync(string voucherShortCode, string validationCode, Guid? sellerCustomerId);
        Task<TransferVoucherError> TransferVoucherAsync(
            string voucherShortCode,
            Guid oldOwnerId,
            Guid newOwnerId);
        Task<VoucherWithValidation> GetByShortCodeAsync(string voucherShortCode);
        Task<VouchersPage> GetCustomerVouchersAsync(Guid customerId, PageInfo pageInfo);
        Task<VouchersPage> GetCampaignVouchersAsync(Guid campaignId, PageInfo pageInfo);

        Task ProcessStuckReservedVouchersAsync(TimeSpan generatePaymentTimeoutPeriod,
            TimeSpan finishPaymentTimeoutPeriod);

        Task MarkVouchersFromExpiredCampaignsAsExpired();
        Task<PresentVouchersResult> PresentVouchersAsync(Guid campaignId, Guid adminId, List<string> customerEmails);
        Task<VoucherWithCampaignInfo> GetSoonestToExpireVoucherAsync(Guid customerId);
        Task<VoucherWithCampaignInfo> GetReservedVoucherAsync(Guid customerId);
    }
}
