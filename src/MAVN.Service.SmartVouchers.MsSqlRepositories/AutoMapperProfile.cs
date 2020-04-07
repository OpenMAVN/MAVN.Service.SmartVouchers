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

            CreateMap<VoucherCampaignContent, VoucherCampaignContentEntity>(MemberList.Destination);

            CreateMap<Voucher, VoucherEntity>(MemberList.Source);
        }
    }
}
