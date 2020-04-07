using AutoMapper;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Entities;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VoucherCampaign, VoucherCampaignEntity>(MemberList.Destination);
            CreateMap<VoucherCampaignEntity, VoucherCampaign>(MemberList.Destination);

            CreateMap<VoucherCampaignContent, VoucherCampaignContentEntity>(MemberList.Destination);
            CreateMap<VoucherCampaignContentEntity, VoucherCampaignContent>(MemberList.Destination)
                .ForMember(e => e.Image, opt => opt.Ignore());

            CreateMap<Voucher, VoucherEntity>(MemberList.Source);

            CreateMap<VoucherEntity, Voucher>(MemberList.Destination);

            CreateMap<VoucherEntity, VoucherWithValidation>(MemberList.Destination)
                .ForMember(e => e.ValidationCode, opt => opt.MapFrom(c => c.Validation.ValidationCode));
        }
    }
}
