using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherBuyModelValidator : AbstractValidator<VoucherBuyModel>
    {
        public VoucherBuyModelValidator()
        {
            RuleFor(x => x.VoucherCampaignId)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.VoucherCampaignId)} required");

            RuleFor(x => x.CustomerId)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.CustomerId)} required");
        }
    }
}
