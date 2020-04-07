using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for smart voucher buying.
    /// </summary>
    [PublicAPI]
    public class VoucherBuyModel
    {
        /// <summary>
        /// Voucher campaign id
        /// </summary>
        public Guid VoucherCampaignId { get; set; }

        /// <summary>
        /// Id of the customer who wants to buy a voucher.
        /// </summary>
        public Guid CustomerId { get; set; }
    }
}
