using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Represents Earn Rule Content model
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignContentEditModel : VoucherCampaignContentCreateModel
    {
        /// <summary>
        /// Represents content's identifier
        /// </summary>
        public Guid Id { get; set; }
    }
}
