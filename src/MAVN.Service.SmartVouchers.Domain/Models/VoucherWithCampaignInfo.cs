using System;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VoucherWithCampaignInfo
    {
        public long Id { get; set; }
        public string ShortCode { get; set; }
        public Guid CampaignId { get; set; }
        public Guid PartnerId { get; set; }
        public VoucherStatus Status { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? RedemptionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CampaignName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
    }
}
