namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VoucherWithValidation : Voucher
    {
        public string ValidationCode { get; set; }
    }
}
