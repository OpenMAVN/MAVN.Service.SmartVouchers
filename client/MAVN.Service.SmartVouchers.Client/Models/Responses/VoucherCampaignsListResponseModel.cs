using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Response model for requested voucher campaigns.
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignsListResponseModel
    {
        /// <summary>
        /// List of Campaigns
        /// </summary>
        public IReadOnlyList<VoucherCampaignResponseModel> Campaigns { get; set; }
    }
}
