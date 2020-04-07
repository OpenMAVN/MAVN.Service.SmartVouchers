using System;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface IVouchersRepository
    {
        Task<long> CreateAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher);
        Task<VouchersPage> GetByCampaignIdAsync(
            Guid campaignId,
            int skip,
            int take);
        Task<VouchersPage> GetByOwnerIdAsync(
            Guid ownerId,
            int skip,
            int take);
    }
}
