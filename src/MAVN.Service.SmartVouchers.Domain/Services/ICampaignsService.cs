using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface ICampaignsService
    {
        Task<Guid> CreateAsync(VoucherCampaign campaign);
        Task<UpdateCampaignError> UpdateAsync(VoucherCampaign campaign);
        Task<bool> DeleteAsync(Guid campaignId);
        Task<VoucherCampaign> GetByIdAsync(Guid campaignId);
        Task<IReadOnlyCollection<VoucherCampaign>> GetCampaignsByIdsAsync(Guid[] campaignIds);
        Task<CampaignsPage> GetCampaignsAsync(CampaignListRequest request);
        Task<ImageSaveError> SaveCampaignContentImage(FileModel file);

        Task<(long publishedCampaingsVouchersCount, long activeCampaingsVouchersCount)>
            GetPublishedAndActiveCampaignsVouchersCountAsync();

        Task MarkCampaignsAsCompletedAsync();
        Task<VoucherCampaign> GetCampaignOfTheDayAsync();
    }
}
