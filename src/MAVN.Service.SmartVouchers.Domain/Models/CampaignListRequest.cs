using System;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class CampaignListRequest
    {
        public string CampaignName { get; set; }
        public bool OnlyActive { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid[] PartnerIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
