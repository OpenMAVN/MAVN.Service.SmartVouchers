using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for voucher campaign editing
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignEditModel : VoucherCampaignBase
    {
        /// <summary>Voucher campaign id</summary>
        public Guid Id { get; set; }

        /// <summary>Voucher campaign state</summary>
        public VoucherCampaignState State { get; set; }

        /// <summary>Voucher campaign contents</summary>
        public List<VoucherCampaignContentEditModel> LocalizedContents { get; set; }
    }
}
