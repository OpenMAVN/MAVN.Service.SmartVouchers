using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    ///  Represents Create voucher campaign content creation request model
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignContentCreateModel
    {
        /// <summary>
        /// Represents the type of the content
        /// </summary>
        public VoucherCampaignContentType ContentType { get; set; }

        /// <summary>
        /// Represents content's language 
        /// </summary>
        public Localization Localization { get; set; }

        /// <summary>
        /// Represents content's value
        /// </summary>
        public string Value { get; set; }
    }
}
