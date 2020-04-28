using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Entities
{
    [Table("payment_request")]
    public class PaymentRequestEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("short_code")]
        [Required]
        public string VoucherShortCode { get; set; }
    }
}
