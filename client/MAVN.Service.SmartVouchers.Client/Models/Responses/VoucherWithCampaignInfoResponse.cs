using System;
using MAVN.Service.SmartVouchers.Client.Models.Responses.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// response model holding info about voucher and corresponding campaign
    /// </summary>
    public class VoucherWithCampaignInfoResponse
    {
        /// <summary>Voucher id</summary>
        public long Id { get; set; }

        /// <summary>Voucher short code</summary>
        public string ShortCode { get; set; }

        /// <summary>Voucher campaign id</summary>
        public Guid CampaignId { get; set; }

        /// <summary>PartnerId id</summary>
        public Guid PartnerId { get; set; }

        /// <summary>Voucher status</summary>
        public SmartVoucherStatus Status { get; set; }

        /// <summary>Voucher owner id</summary>
        public Guid OwnerId { get; set; }

        /// <summary>Voucher purchase date</summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>Voucher redemption date</summary>
        public DateTime? RedemptionDate { get; set; }

        /// <summary>Voucher expiration date</summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>Name of the campaign</summary>
        public string CampaignName { get; set; }

        /// <summary>Url of the image for the campaign</summary>
        public string ImageUrl { get; set; }

        /// <summary>Price of the voucher</summary>
        public decimal Price { get; set; }

        /// <summary>Description</summary>
        public string Description { get; set; }

        /// <summary>Currency</summary>
        public string Currency { get; set; }
    }
}
