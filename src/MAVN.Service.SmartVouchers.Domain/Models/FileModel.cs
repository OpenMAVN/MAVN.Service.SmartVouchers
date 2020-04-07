using System;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class FileModel
    {
        public string Id { get; set; }

        public Guid CampaignId { get; set; }

        public Language Language { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Content { get; set; }
    }
}
