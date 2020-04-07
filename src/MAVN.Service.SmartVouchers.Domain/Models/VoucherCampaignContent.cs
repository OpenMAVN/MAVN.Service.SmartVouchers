using System;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VoucherCampaignContent
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public CampaignContentType ContentType { get; set; }
        public Language Language { get; set; }
        public string Value { get; set; }

        public FileResponse Image { get; set; }
    }
}
