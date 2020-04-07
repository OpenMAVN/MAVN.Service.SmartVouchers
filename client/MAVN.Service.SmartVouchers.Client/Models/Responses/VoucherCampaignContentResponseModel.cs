using System;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Response model for campaign content
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignContentResponseModel
    {
        /// <summary>Represents campaign's content id</summary>
        public Guid Id { get; set; }

        /// <summary>Represents the type of the content</summary>
        public VoucherCampaignContentType ContentType { get; set; }

        /// <summary>Represents content's language</summary>
        public Localization Localization { get; set; }

        /// <summary>Represents content's value</summary>
        public string Value { get; set; }

        /// <summary>Represents content's image</summary>
        public FileResponseModel Image { get; set; }
    }
}
