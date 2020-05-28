using System;
using MAVN.Numerics;

namespace MAVN.Service.SmartVouchers.Contract
{
    public class SmartVoucherUsedEvent
    {
        public DateTime Timestamp { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? LinkedCustomerId { get; set; }

        public Guid PartnerId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid CampaignId { get; set; }

        public Money18 Amount { get; set; }

        public string Currency { get; set; }
    }

}
