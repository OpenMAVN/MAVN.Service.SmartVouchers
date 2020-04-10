using System;
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
    [Route("api/vouchers")]
    public class SmartVouchersController : Controller, ISmartVouchersApi
    {
        private readonly IVouchersService _vouchersService;
        private readonly IMapper _mapper;

        public SmartVouchersController(
            IVouchersService vouchersService,
            IMapper mapper)
        {
            _vouchersService = vouchersService;
            _mapper = mapper;
        }

        /// <summary>
        /// Buys a new voucher from passed voucher campaign.
        /// </summary>
        /// <param name="model">The model that describes voucher buy request.</param>
        [HttpPost]
        [ProducesResponseType(typeof(BuyVoucherErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<BuyVoucherErrorCodes> BuyVoucherAsync([FromBody] VoucherBuyModel model)
        {
            var result = await _vouchersService.BuyVoucherAsync(model.VoucherCampaignId, model.CustomerId);

            return _mapper.Map<BuyVoucherErrorCodes>(result);
        }

        /// <summary>
        /// Get voucher deatils by its short code.
        /// </summary>
        /// <param name="voucherShortCode"></param>
        [HttpGet("{voucherShortCode}")]
        [ProducesResponseType(typeof(VoucherDetailsResponseModel), (int)HttpStatusCode.OK)]
        public async Task<VoucherDetailsResponseModel> GetByShortCodeAsync(string voucherShortCode)
        {
            var result = await _vouchersService.GetByShortCodeAsync(voucherShortCode);

            return _mapper.Map<VoucherDetailsResponseModel>(result);
        }

        /// <summary>
        /// Get smart vouchers for specified voucher campaign.
        /// </summary>
        /// <param name="campaignId">Voucher campaign id.</param>
        /// <param name="pageData">Page data.</param>
        [HttpGet("bycampaign")]
        [ProducesResponseType(typeof(PaginatedVouchersListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<PaginatedVouchersListResponseModel> GetCampaignVouchersAsync(Guid campaignId, [FromQuery] BasePaginationRequestModel pageData)
        {
            var pageInfo = _mapper.Map<PageInfo>(pageData);
            var result = await _vouchersService.GetCampaignVouchersAsync(campaignId, pageInfo);

            return _mapper.Map<PaginatedVouchersListResponseModel>(result);
        }

        /// <summary>
        /// Get smart vouchers for specified customer.
        /// </summary>
        /// <param name="customerId">Customer id.</param>
        /// <param name="pageData">Page data.</param>
        [HttpGet("bycustomer")]
        [ProducesResponseType(typeof(PaginatedVouchersListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<PaginatedVouchersListResponseModel> GetCustomerVouchersAsync(Guid customerId, [FromQuery] BasePaginationRequestModel pageData)
        {
            var pageInfo = _mapper.Map<PageInfo>(pageData);
            var result = await _vouchersService.GetCustomerVouchersAsync(customerId, pageInfo);

            return _mapper.Map<PaginatedVouchersListResponseModel>(result);
        }

        /// <summary>
        /// Redeem a voucher.
        /// </summary>
        /// <param name="model">The model that describes voucher redemption request.</param>
        [HttpPost("usage")]
        [ProducesResponseType(typeof(RedeemVoucherErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<RedeemVoucherErrorCodes> RedeemVoucherAsync([FromBody] VoucherRedeptionModel model)
        {
            var result = await _vouchersService.RedeemVoucherAsync(model.VoucherShortCode, model.VoucherValidationCode);

            return _mapper.Map<RedeemVoucherErrorCodes>(result);
        }

        /// <summary>
        /// Transfer a new voucher to another owner.
        /// </summary>
        /// <param name="model">The model that describes voucher transfer request.</param>
        [HttpPut]
        [ProducesResponseType(typeof(TransferVoucherErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<TransferVoucherErrorCodes> TransferVoucherAsync([FromBody] VoucherTransferModel model)
        {
            var result = await _vouchersService.TransferVoucherAsync(
                model.VoucherShortCode,
                model.OldOwnerId,
                model.NewOwnerId);

            return _mapper.Map<TransferVoucherErrorCodes>(result);
        }

        /// <summary>
        /// Get total count of vouchers for public and active voucher campaigns.
        /// </summary>
        [HttpGet("total")]
        [ProducesResponseType(typeof(PublishedAndActiveCampaignsVouchersCountResponse), (int)HttpStatusCode.OK)]
        public async Task<PublishedAndActiveCampaignsVouchersCountResponse> GetPublishedAndActiveCampaignsVouchersCountAsync()
        {
            var result = await _vouchersService.GetPublishedAndActiveCampaignsVouchersCountAsync();

            return new PublishedAndActiveCampaignsVouchersCountResponse
            {
                ActiveCampaignsVouchersTotalCount = result.activeCampaingsVouchersCount,
                PublishedCampaignsVouchersTotalCount = result.publishedCampaingsVouchersCount
            };
        }
    }
}
