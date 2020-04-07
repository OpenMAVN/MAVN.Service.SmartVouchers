using System;
using AutoMapper;
using MAVN.Service.SmartVouchers.Client.Models.Requests;
using MAVN.Service.SmartVouchers.Client.Models.Responses;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VoucherCampaignCreateModel, VoucherCampaign>(MemberList.Destination)
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.CreationDate, opt => opt.Ignore());
            CreateMap<VoucherCampaignContentCreateModel, VoucherCampaignContent>(MemberList.Destination)
                .ForMember(e => e.Language, opt => opt.MapFrom(c => c.Localization))
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.CampaignId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.Ignore());

            CreateMap<VoucherCampaignEditModel, VoucherCampaign>(MemberList.Destination)
                .ForMember(e => e.CreationDate, opt => opt.Ignore());
            CreateMap<VoucherCampaignContentEditModel, VoucherCampaignContent>(MemberList.Destination)
                .ForMember(e => e.Language, opt => opt.MapFrom(c => c.Localization))
                .ForMember(e => e.CampaignId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.Ignore());

            CreateMap<CampaignsPage, PaginatedVoucherCampaignsListResponseModel>(MemberList.Destination);

            CreateMap<VoucherCampaign, VoucherCampaignDetailsResponseModel>(MemberList.Destination);
            CreateMap<VoucherCampaignContent, VoucherCampaignContentResponseModel>(MemberList.Destination)
                .ForMember(e => e.Localization, opt => opt.MapFrom(c => c.Language));
            CreateMap<FileResponse, FileResponseModel>(MemberList.Destination);

            CreateMap<VoucherCampaign, VoucherCampaignResponseModel>(MemberList.Destination);

            CreateMap<CampaignImageFileRequest, FileModel>(MemberList.Destination)
                .ForMember(e => e.Language, opt => opt.MapFrom(c => c.Localization));
        }
    }
}
