using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for canceling smart voucher reservation.
    /// </summary>
    [PublicAPI]
    public class VoucherCancelReservationModel
    {
        /// <summary>
        /// Voucher short code
        /// </summary>
        public string ShortCode { get; set; }
    }
}
