using System.Collections.Generic;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class CampaignsPage
    {
        public List<VoucherCampaign> Campaigns { get; set; }

        public int TotalCount { get; set; }
    }
}
