using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.MsSql;
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

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task UpdateAsync(Voucher voucher)
        {
            var entity = _mapper.Map<VoucherEntity>(voucher);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.Vouchers.Update(entity);

                await context.SaveChangesAsync();
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
    }
}
