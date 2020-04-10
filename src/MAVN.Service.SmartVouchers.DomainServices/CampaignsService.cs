using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    public class CampaignsService : ICampaignsService
    {
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly ICampaignContentsRepository _campaignContentsRepository;
        private readonly IFileService _fileService;
        private readonly ILog _log;

        public CampaignsService(
            ICampaignsRepository campaignsRepository,
            ICampaignContentsRepository campaignContentsRepository,
            IFileService fileService,
            ILogFactory logFactory)
        {
            _campaignsRepository = campaignsRepository;
            _campaignContentsRepository = campaignContentsRepository;
            _fileService = fileService;
            _log = logFactory.CreateLog(this);
        }

        public async Task<Guid> CreateAsync(VoucherCampaign campaign)
        {
            campaign.CreationDate = DateTime.UtcNow;
            if (campaign.LocalizedContents != null)
                for (int i = 0; i < campaign.LocalizedContents.Count; ++i)
                {
                    if (!string.IsNullOrWhiteSpace(campaign.LocalizedContents[i].Value))
                        continue;

                    campaign.LocalizedContents.RemoveAt(i);
                    --i;
                }

            var result = await _campaignsRepository.CreateAsync(campaign);

            _log.Info("Campaign was added", context: campaign);

            return result;
        }

        public async Task<UpdateCampaignError> UpdateAsync(VoucherCampaign campaign)
        {
            var oldCampaign = await _campaignsRepository.GetByIdAsync(campaign.Id);
            if (oldCampaign == null)
            {
                _log.Error($"Campaign {campaign.Id} not found for update");
                return UpdateCampaignError.VoucherCampaignNotFound;
            }
            if (campaign.VouchersTotalCount < oldCampaign.BoughtVouchersCount)
                return UpdateCampaignError.TotalCountMustBeGreaterThanBoughtVouchersCount;

            campaign.CreatedBy = oldCampaign.CreatedBy;
            campaign.CreationDate = oldCampaign.CreationDate;
            if (campaign.LocalizedContents != null)
                for (int i = 0; i < campaign.LocalizedContents.Count; ++i)
                {
                    if (!string.IsNullOrWhiteSpace(campaign.LocalizedContents[i].Value))
                        continue;

                    campaign.LocalizedContents.RemoveAt(i);
                    --i;
                }

            var contentsToRemove = oldCampaign.LocalizedContents
                .Where(c1 => campaign.LocalizedContents.All(c2 => c1.Id != c2.Id))
                .ToList();
            await _campaignContentsRepository.DeleteAsync(contentsToRemove);
            foreach (var content in contentsToRemove)
            {
                if (content.ContentType == CampaignContentType.ImageUrl)
                {
                    await _fileService.DeleteAsync(content.Id);
                }
            }

            await _campaignsRepository.UpdateAsync(campaign);

            _log.Info("Campaign was updated", context: campaign);

            return UpdateCampaignError.None;
        }

        public async Task<bool> DeleteAsync(Guid campaignId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(campaignId);

            if (campaign == null)
            {
                _log.Info("Campaign does not exists.", campaignId);
                return false;
            }

            await _campaignsRepository.DeleteAsync(campaign);

            foreach (var content in campaign.LocalizedContents)
            {
                await _fileService.DeleteAsync(content.Id);
            }

            _log.Info("Campaign was deleted", campaignId);

            return true;
        }

        public async Task<VoucherCampaign> GetByIdAsync(Guid campaignId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(campaignId);

            if (campaign == null)
                return null;

            foreach (var content in campaign.LocalizedContents)
            {
                if (content.ContentType == CampaignContentType.ImageUrl)
                    content.Image = await _fileService.GetAsync(content.Id);
            }

            return campaign;
        }

        public Task<CampaignsPage> GetCampaignsAsync(CampaignListRequest request)
        {
            return _campaignsRepository.GetCampaignsAsync(request);
        }

        public Task<IReadOnlyCollection<VoucherCampaign>> GetCampaignsByIdsAsync(Guid[] campaignIds)
        {
            return _campaignsRepository.GetCampaignsByIdsAsync(campaignIds);
        }

        public async Task<ImageSaveError> SaveCampaignContentImage(FileModel file)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(file.CampaignId);

            if (campaign == null)
                return ImageSaveError.VoucherCampaignNotFound;

            var campaignContent = campaign.LocalizedContents.FirstOrDefault(c =>
                c.ContentType == CampaignContentType.ImageUrl && c.Language == file.Language);
            if (campaignContent == null)
            {
                campaignContent = new VoucherCampaignContent
                {
                    CampaignId = campaign.Id,
                    ContentType = CampaignContentType.ImageUrl,
                    Language = file.Language,
                };
                campaignContent.Id = await _campaignContentsRepository.CreateAsync(campaignContent);
            }

            var url = await _fileService.SaveAsync(file, campaignContent.Id);
            if (url == null)
                return ImageSaveError.InvalidFileFormat;

            campaignContent.Value = url;
            await _campaignContentsRepository.UpdateAsync(campaignContent);

            return ImageSaveError.None;
        }


        public Task<(int publishedCampaingsVouchersCount, int activeCampaingsVouchersCount)> GetPublishedAndActiveCampaignsVouchersCountAsync()
        {
            return _campaignsRepository.GetPublishedAndActiveCampaignsVouchersCountAsync();
        }
    }
}
