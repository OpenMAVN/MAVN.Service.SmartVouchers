namespace MAVN.Service.SmartVouchers.Domain.Enums
{
    public enum VoucherStatus
    {
        /// <summary>Unspecified status.</summary>
        None,

        /// <summary>Indicates that the voucher is stock.</summary>
        InStock,

        /// <summary>Indicated that the voucher reserved by customer and waiting for payment.</summary>
        Reserved,

        /// <summary>Indicates that the voucher bought by a customer.</summary>
        Sold,

        /// <summary>Indicates that the voucher has been used by a customer/// </summary>
        Used,
    }
}
