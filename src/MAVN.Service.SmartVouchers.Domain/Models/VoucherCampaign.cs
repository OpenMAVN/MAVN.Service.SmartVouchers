using System;
using System.Collections.Generic;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VoucherCampaign
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int VouchersTotalCount { get; set; }
        public int BoughtVouchersCount { get; set; }
        public decimal VoucherPrice { get; set; }
        public string Currency { get; set; }
        public Guid PartnerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public CampaignState State { get; set; }

        public List<VoucherCampaignContent> LocalizedContents { get; set; }
    }
}
