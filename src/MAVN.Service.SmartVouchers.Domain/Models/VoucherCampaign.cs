using System;
using System.Collections.Generic;
using System.Linq;
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
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public CampaignState State { get; set; }

        public List<VoucherCampaignContent> LocalizedContents { get; set; }

        public string GetContent(CampaignContentType contentType, Language language)
        {
            if (LocalizedContents == null)
                return null;

            var contentValue = LocalizedContents
                .FirstOrDefault(o => o.ContentType == contentType && o.Language == language)?.Value;

            if (contentValue != null)
                return contentValue;

            var englishContentValue = LocalizedContents
                .FirstOrDefault(o => o.ContentType == contentType && o.Language == Language.En)?.Value;

            if (englishContentValue != null)
                return englishContentValue;

            return LocalizedContents
                .FirstOrDefault(o => o.ContentType == contentType)?.Value;
        }
    }
}
