using System.Collections.Generic;
using MAVN.Service.SmartVouchers.Client.Models.Responses.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// response model
    /// </summary>
    public class PresentVouchersResponse
    {
        /// <summary>
        /// error
        /// </summary>
        public PresentVouchersErrorCodes Error { get; set; }
        /// <summary>
        /// Not registered emails
        /// </summary>
        public List<string> NotRegisteredEmails { get; set; }
    }
}
