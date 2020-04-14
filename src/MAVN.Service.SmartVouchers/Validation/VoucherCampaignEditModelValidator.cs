using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherCampaignEditModelValidator : VoucherCampaignModelValidatorBase<VoucherCampaignEditModel>
    {
        public VoucherCampaignEditModelValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x != default)
                .WithMessage(x => $"{nameof(x.Id)} required");
        }
    }
}
