using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Entities
{
    [Table("campaign")]
    public class VoucherCampaignEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("vouchers_total_count")]
        public int VouchersTotalCount { get; set; }

        [Column("bought_vouchers_count")]
        public int BoughtVouchersCount { get; set; }

        [Column("voucher_price")]
        public decimal VoucherPrice { get; set; }

        [Column("currency")]
        [Required]
        public string Currency { get; set; }

        [Column("partner_id")]
        [Required]
        public Guid PartnerId { get; set; }

        [Column("from_date")]
        public DateTime FromDate { get; set; }

        [Column("to_date")]
        public DateTime? ToDate { get; set; }

        [Column("expiration_date")]
        public DateTime? ExpirationDate { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by")]
        [Required]
        public string CreatedBy { get; set; }

        [Column("state")]
        public CampaignState State { get; set; }

        public ICollection<VoucherCampaignContentEntity> LocalizedContents { get; set; }
    }
}
