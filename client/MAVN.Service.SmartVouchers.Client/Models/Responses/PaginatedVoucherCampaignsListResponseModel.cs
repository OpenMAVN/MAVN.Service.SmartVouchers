using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Paginated response model for existing voucher campaigns.
    /// </summary>
    [PublicAPI]
    public class PaginatedVoucherCampaignsListResponseModel : BasePaginationResponseModel
    {
        /// <summary>
        /// List of Campaigns
        /// </summary>
        public IReadOnlyList<VoucherCampaignDetailsResponseModel> Campaigns { get; set; }
    }
}
