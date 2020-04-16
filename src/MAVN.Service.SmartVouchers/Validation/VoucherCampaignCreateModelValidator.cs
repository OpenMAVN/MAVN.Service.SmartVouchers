using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.SmartVouchers.Client.Models.Requests;

namespace MAVN.Service.SmartVouchers.Validation
{
    [UsedImplicitly]
    public class VoucherCampaignCreateModelValidator : VoucherCampaignModelValidatorBase<VoucherCampaignCreateModel>
    {
        public VoucherCampaignCreateModelValidator()
        {

            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage(x => $"{nameof(x.CreatedBy)} required");
        }
    }
}
