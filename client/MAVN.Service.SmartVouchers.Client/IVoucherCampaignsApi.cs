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
    public interface IVoucherCampaignsApi
    {
        /// <summary>
        /// Returns list of Campaigns.
        /// </summary>
        /// <param name="request">The model that describes voucher campaigns request model.</param>
        /// <returns>CampaignListModel</returns>
        [Get("/api/campaigns")]
        Task<PaginatedVoucherCampaignsListResponseModel> GetAsync(VoucherCampaignsPaginationRequestModel request);

        /// <summary>
        /// Returns a Campaign by campaignId.
        /// </summary>
        /// <returns>CampaignResponseModel</returns>
        [Get("/api/campaigns/{campaignId}")]
        Task<VoucherCampaignDetailsResponseModel> GetByIdAsync(Guid campaignId);

        /// <summary>
        /// Returns a list of Campaigns by passed campaignIds.
        /// </summary>
        /// <returns>CampaignsInfoListResponseModel</returns>
        [Get("/api/campaigns/ids")]
        Task<VoucherCampaignsListResponseModel> GetCampaignsByIds([Query(CollectionFormat.Multi)] Guid[] voicherCampaignsIds);

        /// <summary>
        /// Adds new voucher campaign.
        /// </summary>
        /// <param name="model">The model that describes voucher campaign model.</param>
        [Post("/api/campaigns")]
        Task<Guid> CreateAsync([Body] VoucherCampaignCreateModel model);

        /// <summary>
        /// Updates existing vooucher campaign.
        /// </summary>
        /// <param name="model">The model that describes voucher campaign model.</param>
        [Put("/api/campaigns")]
        Task<UpdateVoucherCampaignErrorCodes> UpdateAsync([Body] VoucherCampaignEditModel model);

        /// <summary>
        /// Deletes Campaign by identification.
        /// </summary>
        [Delete("/api/campaigns/{campaignId}")]
        Task<bool> DeleteAsync(Guid campaignId);

        /// <summary>
        /// Adds new Campaign's content image
        /// </summary>
        /// <param name="model">The model that describes the file.</param>
        [Post("/api/campaigns/image")]
        Task<SaveImageErrorCodes> SetImage([Body] CampaignImageFileRequest model);
    }
}
