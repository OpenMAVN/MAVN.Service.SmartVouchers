using System;
using AutoMapper;
using MAVN.Service.SmartVouchers.AzureRepositories.Entities;
using MAVN.Service.SmartVouchers.Client.Models.Requests;
using MAVN.Service.SmartVouchers.Client.Models.Responses;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Campaigns
            CreateMap<VoucherCampaignCreateModel, VoucherCampaign>(MemberList.Destination)
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.BoughtVouchersCount, opt => opt.Ignore())
                .ForMember(e => e.State, opt => opt.MapFrom(c => CampaignState.Draft))
                .ForMember(e => e.CreationDate, opt => opt.Ignore());
            CreateMap<VoucherCampaignContentCreateModel, VoucherCampaignContent>(MemberList.Destination)
                .ForMember(e => e.Language, opt => opt.MapFrom(c => c.Localization))
                .ForMember(e => e.Id, opt => opt.MapFrom(c => Guid.NewGuid()))
                .ForMember(e => e.CampaignId, opt => opt.Ignore())
                .ForMember(s => s.Image, opt => opt.Ignore());

            CreateMap<VoucherCampaignEditModel, VoucherCampaign>(MemberList.Destination)
                .ForMember(e => e.BoughtVouchersCount, opt => opt.Ignore())
                .ForMember(e => e.CreatedBy, opt => opt.Ignore())
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

            // Vouchers
            CreateMap<VoucherWithValidation, VoucherDetailsResponseModel>(MemberList.Destination);
            CreateMap<VouchersPage, PaginatedVouchersListResponseModel>(MemberList.Destination);
            CreateMap<Voucher, VoucherResponseModel>(MemberList.Destination);

            CreateMap<BasePaginationRequestModel, PageInfo>(MemberList.Destination);

            CreateMap<FileModel, FileInfoEntity>(MemberList.Destination)
                .ForMember(e => e.CampaignContentId, opt => opt.MapFrom(c => c.Id))
                .ForMember(e => e.ETag, opt => opt.Ignore())
                .ForMember(e => e.PartitionKey, opt => opt.Ignore())
                .ForMember(e => e.RowKey, opt => opt.Ignore())
                .ForMember(e => e.Timestamp, opt => opt.Ignore());

            CreateMap<FileInfoEntity, FileModel>(MemberList.Destination)
                .ForMember(e => e.Id, opt => opt.MapFrom(c => c.CampaignContentId))
                .ForMember(e => e.CampaignId, opt => opt.Ignore())
                .ForMember(e => e.Content, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore());

            CreateMap<VoucherReservationResult, ReserveVoucherResponse>(MemberList.Destination);
        }
    }
}
