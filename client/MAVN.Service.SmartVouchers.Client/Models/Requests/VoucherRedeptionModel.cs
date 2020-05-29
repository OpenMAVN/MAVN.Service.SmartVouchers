using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for smart voucher redemption.
    /// </summary>
    [PublicAPI]
    public class VoucherRedeptionModel
    {
        /// <summary>
        /// Voucher short code
        /// </summary>
        public string VoucherShortCode { get; set; }

        /// <summary>
        /// Voucher validation code
        /// </summary>
        public string VoucherValidationCode { get; set; }

        /// <summary>
        /// Id of the seller
        /// </summary>
        public Guid? SellerCustomerId { get; set; }
    }
}
