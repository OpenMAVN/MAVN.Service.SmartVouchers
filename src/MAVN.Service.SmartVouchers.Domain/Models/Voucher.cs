using System;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class Voucher
    {
        public long Id { get; set; }
        public string ShortCode { get; set; }
        public Guid CampaignId { get; set; }
        public VoucherStatus Status { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? SellerId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? RedemptionDate { get; set; }
    }
}
