using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Detailed reponse model for voucher.
    /// </summary>
    [PublicAPI]
    public class VoucherDetailsResponseModel : VoucherResponseModel
    {
        /// <summary>
        /// Voucher validation code
        /// </summary>
        public string ValidationCode { get; set; }
    }
}
