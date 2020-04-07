using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Entities
{
    [Table("voucher_validation")]
    public class VoucherValidationEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("voucher_id")]
        public long VoucherId { get; set; }

        [Column("validation_code")]
        [Required]
        public string ValidationCode { get; set; }
    }
}
