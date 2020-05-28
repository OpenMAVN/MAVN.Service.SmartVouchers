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
    public class CampaignsRepository : ICampaignsRepository
    {
        private readonly IDbContextFactory<SmartVouchersContext> _contextFactory;
        private readonly IMapper _mapper;

        public CampaignsRepository(
            IDbContextFactory<SmartVouchersContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(VoucherCampaign campaign)
        {
            var entity = _mapper.Map<VoucherCampaignEntity>(campaign);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.VoucherCampaigns.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task UpdateAsync(VoucherCampaign campaign)
        {
            var entity = _mapper.Map<VoucherCampaignEntity>(campaign);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.VoucherCampaigns.Update(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(VoucherCampaign campaign)
        {
            campaign.State = CampaignState.Deleted;

            var entity = _mapper.Map<VoucherCampaignEntity>(campaign);

            using (var context = _contextFactory.CreateDataContext())
            {
                context.VoucherCampaigns.Update(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task<VoucherCampaign> GetByIdAsync(Guid campaignId, bool includeContents)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = context.VoucherCampaigns
                    .Where(c => c.Id == campaignId);

                if (includeContents)
                    query = query.Include(c => c.LocalizedContents);

                var entity = await query.FirstOrDefaultAsync();

                return _mapper.Map<VoucherCampaign>(entity);
            }
        }

        public async Task<IReadOnlyCollection<VoucherCampaign>> GetCampaignsByIdsAsync(Guid[] campaignIds)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var query = await context.VoucherCampaigns.AsNoTracking()
                    .Where(c => campaignIds.Contains(c.Id))
                    .Include(c => c.LocalizedContents)
                    .ToListAsync();

                return _mapper.Map<List<VoucherCampaign>>(query);
            }
        }

        public async Task<CampaignsPage> GetCampaignsAsync(CampaignListRequest request)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var hasPartnersFilter = request.PartnerIds != null && request.PartnerIds.Any();
                var query = context.VoucherCampaigns
                    .Where(c => c.State != CampaignState.Deleted);

                if (hasPartnersFilter)
                {
                    query = query.Where(c => request.PartnerIds.Contains(c.PartnerId));
                }

                if (!string.IsNullOrWhiteSpace(request.CampaignName))
                {
                    var name = request.CampaignName.Trim().ToLower();
                    query = query.Where(c => c.Name.ToLower().Contains(name));
                }

                if (request.CampaignState.HasValue)
                {
                    query = query.Where(c => c.State == request.CampaignState.Value);
                }

                if (request.OnlyActive)
                {
                    var now = DateTime.UtcNow;
                    query = query.Where(c =>
                        c.FromDate <= now
                        && (!c.ToDate.HasValue || c.ToDate.Value > now)
                        && c.BoughtVouchersCount < c.VouchersTotalCount);
                }

                if (request.CreatedBy.HasValue && request.CreatedBy.Value != Guid.Empty)
                {
                    query = query.Where(p => p.CreatedBy == request.CreatedBy.ToString());
                }

                query = query
                    .Include(c => c.LocalizedContents)
                    .AsNoTracking();

                List<VoucherCampaignEntity> result;

                if (hasPartnersFilter && request.PartnerIds.Length > 1)
                {
                    var partnersOrders = new Dictionary<Guid, int>();
                    for (var i = 0; i < request.PartnerIds.Length; i++)
                    {
                        partnersOrders.Add(request.PartnerIds[i], i);
                    }
                    //This is done in memory because we cannot apply this ordering and paging on db level (can't be translated)
                    result = await query.ToListAsync();
                    result = result
                        .OrderBy(p => partnersOrders[p.PartnerId])
                        .Skip(request.Skip)
                        .Take(request.Take)
                        .ToList();
                }
                else
                {
                    result = await query
                        .OrderByDescending(i => i.CreationDate)
                        .Skip(request.Skip)
                        .Take(request.Take)
                        .ToListAsync();
                }
                var totalCount = await query.CountAsync();

                return new CampaignsPage
                {
                    Campaigns = _mapper.Map<List<VoucherCampaign>>(result),
                    TotalCount = totalCount,
                };
            }
        }

        public async Task<(long publishedCampaingsVouchersCount, long activeCampaingsVouchersCount)> GetPublishedAndActiveCampaignsVouchersCountAsync()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var now = DateTime.UtcNow;

                var publishedCampaignsVouchersCount = await context.VoucherCampaigns
                    .Where(c => c.State == CampaignState.Published)
                    .Select(c => (long)c.VouchersTotalCount)
                    .SumAsync();

                var activeCampaignsVouchersCount = await context.VoucherCampaigns
                    .Where(c => c.State == CampaignState.Published && c.FromDate <= now &&
                                (c.ToDate == null || c.ToDate > now))
                    .Select(c => (long)c.VouchersTotalCount)
                    .SumAsync();

                return (publishedCampaignsVouchersCount, activeCampaignsVouchersCount);
            }
        }

        public async Task<Guid[]> GetFinishedCampaignsIdsAsync()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var now = DateTime.UtcNow;

                var result = await context.VoucherCampaigns
                    .Where(c => c.ToDate <= now)
                    .Select(x => x.Id)
                    .ToArrayAsync();

                return result;
            }
        }
    }
}
