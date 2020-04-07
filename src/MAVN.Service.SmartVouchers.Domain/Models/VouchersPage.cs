using System.Collections.Generic;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class VouchersPage
    {
        public List<Voucher> Vouchers { get; set; }

        public int TotalCount { get; set; }
    }
}
