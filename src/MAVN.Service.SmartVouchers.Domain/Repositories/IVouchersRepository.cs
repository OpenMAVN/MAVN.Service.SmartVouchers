using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface IVouchersRepository
    {
        Task<long> CreateAsync(Voucher voucher);
        Task ReserveAsync(Voucher voucher);
        Task CancelReservationAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher, string validationCode = null);
        Task<VoucherWithValidation> GetByShortCodeAsync(string shortCode);
        Task<VouchersPage> GetByCampaignIdAsync(
            Guid campaignId,
            int skip,
            int take);
        Task<VouchersPage> GetByOwnerIdAsync(
            Guid ownerId,
            int skip,
            int take);
        Task<List<Voucher>> GetByCampaignIdAndStatusAsync(Guid campaignId, VoucherStatus status);
        Task<Voucher> GetReservedByCampaignIdAndOwnerAsync(Guid campaignId, Guid owner);
    }
}
