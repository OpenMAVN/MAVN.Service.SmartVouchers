using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories
{
    public class CampaignContentsRepository : ICampaignContentsRepository
    {
        private readonly IDbContextFactory<SmartVouchersContext> _contextFactory;
        private readonly IMapper _mapper;

        public CampaignContentsRepository(
            IDbContextFactory<SmartVouchersContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory
                ?? throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(VoucherCampaignContent campaignContent)
        {
            var entity = _mapper.Map<VoucherCampaignContentEntity>(campaignContent);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.CampaignsContents.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task DeleteAsync(IEnumerable<VoucherCampaignContent> campaignContents)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<VoucherCampaignContentEntity>>(campaignContents);

                foreach (var entity in entities)
                {
                    context.CampaignsContents.Remove(entity);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<VoucherCampaignContent> GetAsync(Guid contentId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.CampaignsContents
                    .FirstOrDefaultAsync(e => e.Id == contentId);

                return _mapper.Map<VoucherCampaignContent>(entity);
            }
        }

        public async Task UpdateAsync(VoucherCampaignContent campaignContent)
        {
            var entity = _mapper.Map<VoucherCampaignContentEntity>(campaignContent);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.CampaignsContents.Update(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}
