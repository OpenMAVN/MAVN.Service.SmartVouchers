using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VoucherReservationResult
    {
        public ProcessingVoucherError ErrorCode { get; set; }

        public string PaymentUrl { get; set; }

        public string AlreadyReservedVoucherShortCode { get; set; }
    }
}
