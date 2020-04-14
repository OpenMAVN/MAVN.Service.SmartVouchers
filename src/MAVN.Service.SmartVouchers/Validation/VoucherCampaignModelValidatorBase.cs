using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherCampaignModelValidatorBase<T> : AbstractValidator<T>
        where T: VoucherCampaignCreateModel
    {
        public VoucherCampaignModelValidatorBase()
        {
            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.Currency)} required");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.Name)} required");

            RuleFor(x => x.PartnerId)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.PartnerId)} required");

            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.CreatedBy)} required");

            RuleFor(x => x.VouchersTotalCount)
                .Must(x => x > 0)
                .WithMessage(x => $"{nameof(x.VouchersTotalCount)} must be positive");

            RuleFor(x => x.VoucherPrice)
                .Must(x => x >= 0)
                .WithMessage(x => $"{nameof(x.VoucherPrice)} must be non-negative");

            RuleFor(x => x.FromDate)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.FromDate)} required");
        }
    }
}
