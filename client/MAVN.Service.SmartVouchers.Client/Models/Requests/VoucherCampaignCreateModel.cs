using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for voucher campaign creation
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignCreateModel : VoucherCampaignBase
    {
        /// <summary>Voucher campaign's author</summary>
        public string CreatedBy { get; set; }

        /// <summary>Voucher campaign contents</summary>
        public List<VoucherCampaignContentCreateModel> LocalizedContents { get; set; }
    }
}
