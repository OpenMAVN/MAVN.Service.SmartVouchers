using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Response model for voucher campaign details.
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignDetailsResponseModel : VoucherCampaignResponseModel
    {
        /// <summary>
        /// Voucher campaigns contents.
        /// </summary>
        public List<VoucherCampaignContentResponseModel> LocalizedContents { get; set; }
    }
}
