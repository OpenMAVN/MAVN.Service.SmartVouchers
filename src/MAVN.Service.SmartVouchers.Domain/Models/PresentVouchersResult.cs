using System.Collections.Generic;
using MAVN.Service.SmartVouchers.Domain.Enums;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class PresentVouchersResult
    {
        public PresentVouchersErrorCodes Error { get; set; }
        public List<string> NotRegisteredEmails { get; set; }
    }
}
