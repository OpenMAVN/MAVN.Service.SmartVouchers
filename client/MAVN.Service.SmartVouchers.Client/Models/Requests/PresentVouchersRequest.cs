using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model to present vouchers
    /// </summary>
    public class PresentVouchersRequest
    {
        /// <summary>
        /// Id of the campaign
        /// </summary>
        [Required]
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Id of the admin
        /// </summary>
        [Required]
        public Guid AdminId { get; set; }

        /// <summary>
        /// Emails of customer receivers
        /// </summary>
        [Required]
        public List<string> CustomerEmails { get; set; }
    }
}
