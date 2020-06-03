using System;

namespace MAVN.Service.SmartVouchers.Contract
{
    public class SmartVoucherTransferredEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Customer id of the sender
        /// </summary>
        public Guid OldCustomerId { get; set; }

        /// <summary>
        /// Customer id of the receiver
        /// </summary> 
        public Guid NewCustomerId { get; set; }

        /// <summary>
        /// Partner id
        /// </summary>
        public Guid PartnerId { get; set; }

        /// <summary>
        /// Id of the smart voucher campaign
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Short code of the voucher
        /// </summary>
        public string VoucherShortCode { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }
    }
}
