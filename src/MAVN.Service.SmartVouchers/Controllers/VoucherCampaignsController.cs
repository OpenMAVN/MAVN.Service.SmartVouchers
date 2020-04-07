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
        [ProducesResponseType(typeof(VoucherCampaignErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignErrorCodes> CreateAsync([FromBody] VoucherCampaignCreateModel model)
        {
            var campaign = _mapper.Map<VoucherCampaign>(model);

            await _campaignsService.CreateAsync(campaign);

            return VoucherCampaignErrorCodes.None;
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully updated.</response>
        [HttpPut]
        [ProducesResponseType(typeof(VoucherCampaignErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignErrorCodes> UpdateAsync([FromBody] VoucherCampaignEditModel model)
        {
            var campaign = _mapper.Map<VoucherCampaign>(model);

            var error = await _campaignsService.UpdateAsync(campaign);

            return _mapper.Map<VoucherCampaignErrorCodes>(error);
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully deleted.</response>
        [HttpDelete("{campaignId}")]
        [ProducesResponseType(typeof(VoucherCampaignErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignErrorCodes> DeleteAsync(Guid campaignId)
        {
            await _campaignsService.DeleteAsync(campaignId);

            return VoucherCampaignErrorCodes.None;
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
                Skip = (request.CurrentPage - 1) * request.PageSize,
                Take = request.PageSize,
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
            var campaigns = await _campaignsService.GetCampaignsByIdsAsync(voicherCampaignsIds);

            return new VoucherCampaignsListResponseModel
            {
                Campaigns = _mapper.Map<IReadOnlyList<VoucherCampaignResponseModel>>(campaigns),
            };
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign's image is successfully updated.</response>
        [HttpPost("image")]
        [ProducesResponseType(typeof(VoucherCampaignErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<VoucherCampaignErrorCodes> SetImage([FromBody] CampaignImageFileRequest model)
        {
            var file = _mapper.Map<FileModel>(model);

            var error = await _campaignsService.SaveCampaignContentImage(file);

            return _mapper.Map<VoucherCampaignErrorCodes>(error);
        }
    }
}
