using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Service.SmartVouchers.Client;
using MAVN.Service.SmartVouchers.Client.Models.Requests;
using MAVN.Service.SmartVouchers.Client.Models.Responses;
using MAVN.Service.SmartVouchers.Client.Models.Responses.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.SmartVouchers.Controllers
{
    [Route("api/campaigns")]
    public class VoucherCampaignsController : Controller, IVoucherCampaignsApi
    {
        private readonly ICampaignsService _campaignsService;
        private readonly IMapper _mapper;

        public VoucherCampaignsController(
            ICampaignsService campaignsService,
            IMapper mapper)
        {
            _campaignsService = campaignsService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public Task<Guid> CreateAsync([FromBody] VoucherCampaignCreateModel model)
        {
            var campaign = _mapper.Map<VoucherCampaign>(model);

            return _campaignsService.CreateAsync(campaign);
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully updated.</response>
        [HttpPut]
        [ProducesResponseType(typeof(UpdateVoucherCampaignErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<UpdateVoucherCampaignErrorCodes> UpdateAsync([FromBody] VoucherCampaignEditModel model)
        {
            var campaign = _mapper.Map<VoucherCampaign>(model);

            var error = await _campaignsService.UpdateAsync(campaign);

            return _mapper.Map<UpdateVoucherCampaignErrorCodes>(error);
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully deleted.</response>
        [HttpDelete("{campaignId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public Task<bool> DeleteAsync(Guid campaignId)
        {
            if (campaignId == default)
                throw new ArgumentNullException();

            return _campaignsService.DeleteAsync(campaignId);
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of campaigns.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedVoucherCampaignsListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<PaginatedVoucherCampaignsListResponseModel> GetAsync(VoucherCampaignsPaginationRequestModel request)
        {
            var campaignListRequestModel = new CampaignListRequest
            {
                CampaignName = request.CampaignName,
                OnlyActive = request.OnlyActive,
                CreatedBy = request.CreatedBy,
                Skip = (request.CurrentPage - 1) * request.PageSize,
                Take = request.PageSize,
                PartnerIds = request.PartnerIds,
            };

            var campaignsPaged = await _campaignsService.GetCampaignsAsync(campaignListRequestModel);

            var campaignModels = _mapper.Map<PaginatedVoucherCampaignsListResponseModel>(campaignsPaged);

            return campaignModels;
        }

        /// <inheritdoc/>
        /// <response code="200">Campaign.</response>
        [HttpGet("{campaignId}")]
        [ProducesResponseType(typeof(VoucherCampaignResponseModel), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignDetailsResponseModel> GetByIdAsync(Guid campaignId)
        {
            if (campaignId == default)
                throw new ArgumentNullException();

            var campaign = await _campaignsService.GetByIdAsync(campaignId);

            var campaignModel = _mapper.Map<VoucherCampaignDetailsResponseModel>(campaign);

            return campaignModel;
        }

        /// <inheritdoc/>
        /// <response code="200">CampaignsInfoListResponseModel.</response>
        [HttpGet("ids")]
        [ProducesResponseType(typeof(VoucherCampaignsListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignsListResponseModel> GetCampaignsByIds([FromQuery] Guid[] voicherCampaignsIds)
        {
            if (voicherCampaignsIds == null || voicherCampaignsIds.Length == 0)
                throw new ArgumentNullException();

            var campaigns = await _campaignsService.GetCampaignsByIdsAsync(voicherCampaignsIds);

            return new VoucherCampaignsListResponseModel
            {
                Campaigns = _mapper.Map<IReadOnlyList<VoucherCampaignDetailsResponseModel>>(campaigns),
            };
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign's image is successfully updated.</response>
        [HttpPost("image")]
        [ProducesResponseType(typeof(SaveImageErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<SaveImageErrorCodes> SetImage([FromBody] CampaignImageFileRequest model)
        {
            var file = _mapper.Map<FileModel>(model);

            var error = await _campaignsService.SaveCampaignContentImage(file);

            return _mapper.Map<SaveImageErrorCodes>(error);
        }


        /// <summary>
        /// Get total count of vouchers for public and active voucher campaigns.
        /// </summary>
        [HttpGet("totalvouchers")]
        [ProducesResponseType(typeof(PublishedAndActiveCampaignsVouchersCountResponse), (int)HttpStatusCode.OK)]
        public async Task<PublishedAndActiveCampaignsVouchersCountResponse> GetPublishedAndActiveCampaignsVouchersCountAsync()
        {
            var result = await _campaignsService.GetPublishedAndActiveCampaignsVouchersCountAsync();

            return new PublishedAndActiveCampaignsVouchersCountResponse
            {
                ActiveCampaignsVouchersTotalCount = result.activeCampaingsVouchersCount,
                PublishedCampaignsVouchersTotalCount = result.publishedCampaingsVouchersCount
            };
        }
    }
}
