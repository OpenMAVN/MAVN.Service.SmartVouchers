using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface ICampaignContentsRepository
    {
        Task<Guid> CreateAsync(VoucherCampaignContent campaignContent);
        Task UpdateAsync(VoucherCampaignContent campaignContent);
        Task DeleteAsync(IEnumerable<VoucherCampaignContent> campaignContents);
        Task<VoucherCampaignContent> GetAsync(Guid contentId);
    }
}
