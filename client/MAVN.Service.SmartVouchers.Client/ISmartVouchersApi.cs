using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;
using MAVN.Service.SmartVouchers.Client.Models.Responses;
using MAVN.Service.SmartVouchers.Client.Models.Responses.Enums;
using Refit;

namespace MAVN.Service.SmartVouchers.Client
{
    /// <summary>
    /// Vouchers campaigns API interface.
    /// </summary>
    [PublicAPI]
    public interface ISmartVouchersApi
    {
        /// <summary>
        /// Reserve a new voucher from passed voucher campaign.
        /// </summary>
        /// <param name="model">The model that describes voucher reserve request.</param>
        [Post("/api/vouchers/reserve")]
        Task<ReserveVoucherResponse> ReserveVoucherAsync([Body] VoucherProcessingModel model);

        /// <summary>
        /// Cancel voucher reservation.
        /// </summary>
        /// <param name="model">The model that describes voucher canceling reservation request.</param>
        [Post("/api/vouchers/cancelReservation")]
        Task<ProcessingVoucherErrorCodes> CancelVoucherReservationAsync([Body] VoucherCancelReservationModel model);

        /// <summary>
        /// Redeem a voucher.
        /// </summary>
        /// <param name="model">The model that describes voucher redemption request.</param>
        [Post("/api/vouchers/usage")]
        Task<RedeemVoucherErrorCodes> RedeemVoucherAsync([Body] VoucherRedeptionModel model);

        /// <summary>
        /// Transfer a new voucher to another owner.
        /// </summary>
        /// <param name="model">The model that describes voucher transfer request.</param>
        [Put("/api/vouchers")]
        Task<TransferVoucherErrorCodes> TransferVoucherAsync([Body] VoucherTransferModel model);

        /// <summary>
        /// Get voucher deatils by its short code.
        /// </summary>
        /// <param name="voucherShortCode"></param>
        [Get("/api/vouchers/{voucherShortCode}")]
        Task<VoucherDetailsResponseModel> GetByShortCodeAsync(string voucherShortCode);

        /// <summary>
        /// Get smart vouchers for specified customer.
        /// </summary>
        /// <param name="customerId">Customer id.</param>
        /// <param name="pageData">Page data.</param>
        [Get("/api/vouchers/bycustomer")]
        Task<PaginatedVouchersListResponseModel> GetCustomerVouchersAsync(Guid customerId, [Query] BasePaginationRequestModel pageData);

        /// <summary>
        /// Get smart vouchers for specified voucher campaign.
        /// </summary>
        /// <param name="campaignId">Voucher campaign id.</param>
        /// <param name="pageData">Page data.</param>
        [Get("/api/vouchers/bycampaign")]
        Task<PaginatedVouchersListResponseModel> GetCampaignVouchersAsync(Guid campaignId, [Query] BasePaginationRequestModel pageData);
    }
}
