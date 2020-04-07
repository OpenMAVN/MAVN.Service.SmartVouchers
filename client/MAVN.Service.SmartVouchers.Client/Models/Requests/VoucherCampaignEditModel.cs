using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Enums;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Request model for voucher campaign editing
    /// </summary>
    [PublicAPI]
    public class VoucherCampaignEditModel
    {
        /// <summary>Voucher campaign id</summary>
        public Guid Id { get; set; }

        /// <summary>Voucher campaign name</summary>
        public string Name { get; set; }

        /// <summary>Voucher campaign description</summary>
        public string Description { get; set; }
        /// <summary>Total vouchers count</summary>
        public int VouchersTotalCount { get; set; }

        /// <summary>Bought vouchers count</summary>
        public int BoughtVouchersCount { get; set; }

        /// <summary>Voucher price</summary>
        public decimal VoucherPrice { get; set; }

        /// <summary>Voucher price currency</summary>
        public string Currency { get; set; }

        /// <summary>Voucher campaign issuer</summary>
        public string PartnerId { get; set; }

        /// <summary>Voucher campaign start date</summary>
        public DateTime FromDate { get; set; }

        /// <summary>Voucher campaign end date</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>Voucher campaign's author</summary>
        public string CreatedBy { get; set; }

        /// <summary>Voucher campaign state</summary>
        public VoucherCampaignState State { get; set; }

        /// <summary>Voucher campaign contents</summary>
        public List<VoucherCampaignContentEditModel> LocalizedContents { get; set; }
    }
}
