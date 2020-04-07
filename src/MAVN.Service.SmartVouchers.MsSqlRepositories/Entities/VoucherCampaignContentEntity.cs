using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Entities
{
    [Table("campaign_content")]
    public class VoucherCampaignContentEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("campaign_id")]
        public Guid CampaignId { get; set; }

        [Column("content_type")]
        public CampaignContentType ContentType { get; set; }

        [Column("language")]
        public Language Language { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
