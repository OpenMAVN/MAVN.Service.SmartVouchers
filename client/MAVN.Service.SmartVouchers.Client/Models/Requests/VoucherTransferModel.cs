using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for smart voucher transfer.
    /// </summary>
    [PublicAPI]
    public class VoucherTransferModel
    {
        /// <summary>Voucher short code</summary>
        public string VoucherShortCode { get; set; }

        /// <summary>Old owner id</summary>
        public Guid OldOwnerId { get; set; }

        /// <summary>New owner id</summary>
        public Guid NewOwnerId { get; set; }
    }
}
