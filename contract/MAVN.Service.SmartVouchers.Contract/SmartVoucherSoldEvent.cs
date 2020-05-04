using System;

namespace MAVN.Service.SmartVouchers.Contract
{
    /// <summary>
    /// Event which notifies that a smart voucher has been sold
    /// </summary>
    public class SmartVoucherSoldEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Customer id
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Partner id
        /// </summary>
        public Guid PartnerId { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Id of the smart voucher campaign
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Short code of the voucher
        /// </summary>
        public string VoucherShortCode { get; set; }

    }
}
