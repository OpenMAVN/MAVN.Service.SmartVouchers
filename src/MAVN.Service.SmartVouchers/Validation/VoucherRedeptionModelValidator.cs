using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherRedeptionModelValidator : AbstractValidator<VoucherRedeptionModel>
    {
        public VoucherRedeptionModelValidator()
        {
            RuleFor(x => x.VoucherShortCode)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.VoucherShortCode)} required");

            RuleFor(x => x.VoucherValidationCode)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.VoucherValidationCode)} required");
        }
    }
}
