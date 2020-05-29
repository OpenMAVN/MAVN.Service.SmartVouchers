using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Entities
{
    [Table("voucher")]
    public class VoucherEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("short_code")]
        public string ShortCode { get; set; }

        [Column("seller_id")]
        public Guid? SellerId { get; set; }

        [Column("campaign_id")]
        [Required]
        public Guid CampaignId { get; set; }

        [Column("status")]
        public VoucherStatus Status { get; set; }

        [Column("owner_id")]
        public Guid? OwnerId { get; set; }

        [Column("purchase_date")]
        public DateTime? PurchaseDate { get; set; }

        [Column("redemption_date")]
        public DateTime? RedemptionDate { get; set; }

        public VoucherValidationEntity Validation { get; set; }
    }
}
