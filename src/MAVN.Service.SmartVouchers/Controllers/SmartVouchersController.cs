using System;
using System.ComponentModel.DataAnnotations;
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
        /// Reserve a new voucher from passed voucher campaign.
        /// </summary>
        /// <param name="model">The model that describes voucher reserve request.</param>
        [HttpPost("reserve")]
        [ProducesResponseType(typeof(ReserveVoucherResponse), (int)HttpStatusCode.OK)]
        public async Task<ReserveVoucherResponse> ReserveVoucherAsync([FromBody] VoucherProcessingModel model)
        {
            var result = await _vouchersService.ReserveVoucherAsync(model.VoucherCampaignId, model.CustomerId);

            return _mapper.Map<ReserveVoucherResponse>(result);
        }

        /// <summary>
        /// Cancel voucher reservation.
        /// </summary>
        /// <param name="model">The model that describes voucher canceling reservation request.</param>
        [HttpPost("cancelReservation")]
        [ProducesResponseType(typeof(ProcessingVoucherErrorCodes), (int)HttpStatusCode.OK)]
        public async Task<ProcessingVoucherErrorCodes> CancelVoucherReservationAsync([FromBody] VoucherCancelReservationModel model)
        {
            var result = await _vouchersService.CancelVoucherReservationAsync(model.ShortCode);

            return _mapper.Map<ProcessingVoucherErrorCodes>(result);
        }

        /// <summary>
        /// Get voucher deatils by its short code.
        /// </summary>
        /// <param name="voucherShortCode"></param>
        [HttpGet("{voucherShortCode}")]
        [ProducesResponseType(typeof(VoucherDetailsResponseModel), (int)HttpStatusCode.OK)]
        public async Task<VoucherDetailsResponseModel> GetByShortCodeAsync(string voucherShortCode)
        {
            if (string.IsNullOrWhiteSpace(voucherShortCode))
                throw new ArgumentNullException();

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
            if (campaignId == default)
                throw new ArgumentNullException(nameof(campaignId));

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
            if (customerId == default)
                throw new ArgumentNullException(nameof(customerId));

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
        public async Task<RedeemVoucherErrorCodes> RedeemVoucherAsync([FromBody][Required] VoucherRedeptionModel model)
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
    }
}
