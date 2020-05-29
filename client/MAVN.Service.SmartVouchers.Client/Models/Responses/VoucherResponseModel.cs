using System;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Responses.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Smart voucher response model
    /// </summary>
    [PublicAPI]
    public class VoucherResponseModel
    {
        /// <summary>Voucher id</summary>
        public long Id { get; set; }

        /// <summary>Voucher short code</summary>
        public string ShortCode { get; set; }

        /// <summary>Voucher campaign id</summary>
        public Guid CampaignId { get; set; }

        /// <summary>Voucher status</summary>
        public SmartVoucherStatus Status { get; set; }

        /// <summary>Voucher owner id</summary>
        public Guid OwnerId { get; set; }

        /// <summary>Voucher purchase date</summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>Voucher redemption date</summary>
        public DateTime? RedemptionDate { get; set; }
    }
}
