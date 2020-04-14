using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherTransferModelValidator : AbstractValidator<VoucherTransferModel>
    {
        public VoucherTransferModelValidator()
        {
            RuleFor(x => x.VoucherShortCode)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.VoucherShortCode)} required");

            RuleFor(x => x.OldOwnerId)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.OldOwnerId)} required");

            RuleFor(x => x.NewOwnerId)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.NewOwnerId)} required");
        }
    }
}
