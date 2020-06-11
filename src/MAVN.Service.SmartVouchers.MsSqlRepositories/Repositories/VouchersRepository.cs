using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Common.MsSql;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories
{
    public class VouchersRepository : IVouchersRepository
    {
        private readonly IDbContextFactory<SmartVouchersContext> _contextFactory;
        private readonly IMapper _mapper;

        public VouchersRepository(
            IDbContextFactory<SmartVouchersContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<long> CreateAsync(Voucher voucher)
        {
            var entity = _mapper.Map<VoucherEntity>(voucher);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.Vouchers.Add(entity);

                var campaign = await context.VoucherCampaigns.FindAsync(entity.CampaignId);
                campaign.BoughtVouchersCount++;
                context.VoucherCampaigns.Update(campaign);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task ReserveAsync(Voucher voucher, Guid ownerId)
        {
            var entity = _mapper.Map<VoucherEntity>(voucher);

            using (var context = _contextFactory.CreateDataContext())
            {
                entity.OwnerId = ownerId;
                entity.Status = VoucherStatus.Reserved;
                entity.PurchaseDate = DateTime.UtcNow;

                context.Vouchers.Update(entity);

                var campaign = await context.VoucherCampaigns.FindAsync(entity.CampaignId);
                campaign.BoughtVouchersCount++;
                context.VoucherCampaigns.Update(campaign);

                await context.SaveChangesAsync();
            }
        }

        public async Task CancelReservationAsync(Voucher voucher)
        {
            var entity = _mapper.Map<VoucherEntity>(voucher);

            using (var context = _contextFactory.CreateDataContext())
            {
                entity.Status = VoucherStatus.InStock;
                entity.OwnerId = null;
                entity.PurchaseDate = null;

                context.Vouchers.Update(entity);

                var campaign = await context.VoucherCampaigns.FindAsync(entity.CampaignId);
                campaign.BoughtVouchersCount--;
                context.VoucherCampaigns.Update(campaign);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Voucher voucher, string validationCode = null)
        {
            var entity = _mapper.Map<VoucherEntity>(voucher);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.Vouchers.Update(entity);

                if (!string.IsNullOrWhiteSpace(validationCode))
                {
                    var validation = await context.VoucherValidations.FirstOrDefaultAsync(v => v.VoucherId == entity.Id);
                    if (validation != null)
                        validation.ValidationCode = validationCode;
                    else
                        context.VoucherValidations.Add(
                            new VoucherValidationEntity { ValidationCode = validationCode, VoucherId = entity.Id });
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<VoucherWithValidation> GetWithValidationByShortCodeAsync(string shortCode)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Vouchers
                    .Where(v => v.ShortCode == shortCode)
                    .Include(v => v.Validation)
                    .FirstOrDefaultAsync();

                return _mapper.Map<VoucherWithValidation>(entity);
            }
        }

        public async Task<Voucher> GetByShortCodeAsync(string shortCode)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.Vouchers
                    .Where(v => v.ShortCode == shortCode)
                    .FirstOrDefaultAsync();

                return _mapper.Map<Voucher>(entity);
            }
        }

        public async Task<VouchersPage> GetByCampaignIdAsync(
            Guid campaignId,
            int skip,
            int take)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = context.Vouchers.Where(v => v.CampaignId == campaignId);

                var result = await query
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                var totalCount = await query.CountAsync();

                return new VouchersPage
                {
                    Vouchers = _mapper.Map<List<Voucher>>(result),
                    TotalCount = totalCount,
                };
            }
        }

        public async Task<VouchersPage> GetByOwnerIdAsync(
            Guid ownerId,
            int skip,
            int take)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = context.Vouchers.Where(v => v.OwnerId == ownerId);

                var result = await query
                    .OrderByDescending(x => x.PurchaseDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                var totalCount = await query.CountAsync();

                return new VouchersPage
                {
                    Vouchers = _mapper.Map<List<Voucher>>(result),
                    TotalCount = totalCount,
                };
            }
        }

        public async Task<List<Voucher>> GetByCampaignIdAndStatusAsync(
            Guid campaignId, VoucherStatus status)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = context.Vouchers.Where(v => v.CampaignId == campaignId && v.Status == status);
                var result = await query.ToListAsync();
                return _mapper.Map<List<Voucher>>(result);
            }
        }

        public async Task<List<Voucher>> GetReservedVouchersBeforeDateAsync(DateTime reservationTimeoutDate)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = context.Vouchers.Where(v =>
                    v.Status == VoucherStatus.Reserved && v.PurchaseDate.Value < reservationTimeoutDate);
                var vouchers = await query.ToListAsync();

                return _mapper.Map<List<Voucher>>(vouchers);
            }
        }

        public async Task SetVouchersFromCampaignsAsExpired(Guid[] campaignsIds)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var vouchers = await context.Vouchers
                    .Where(v => campaignsIds.Contains(v.CampaignId))
                    .ToListAsync();

                foreach (var voucher in vouchers)
                {
                    voucher.Status = VoucherStatus.Expired;
                    context.Vouchers.Update(voucher);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> AnyReservedVouchersAsync(Guid customerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.Vouchers
                    .AnyAsync(x => x.OwnerId == customerId && x.Status == VoucherStatus.Reserved);

                return result;
            }
        }
    }
}
