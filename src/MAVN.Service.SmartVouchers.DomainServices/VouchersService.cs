using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    public class VouchersService : IVouchersService
    {
        private readonly IVouchersRepository _vouchersRepository;
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly ILog _log;

        public VouchersService(
            IVouchersRepository vouchersRepository,
            ICampaignsRepository campaignsRepository,
            ILogFactory logFactory)
        {
            _vouchersRepository = vouchersRepository;
            _campaignsRepository = campaignsRepository;
            _log = logFactory.CreateLog(this);
        }

        public async Task<BuyVoucherError> BuyVoucherAsync(Guid voucherCampaignId, Guid ownerId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(voucherCampaignId);
            if (campaign == null)
                return BuyVoucherError.VoucherCampaignNotFound;

            if (campaign.State != CampaignState.Published
                || DateTime.UtcNow < campaign.FromDate
                || campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow)
                return BuyVoucherError.VoucherCampaignNotActive;

            if (campaign.VouchersTotalCount <= campaign.BoughtVouchersCount)
                return BuyVoucherError.NoAvailableVouchers;

            var (validationCode, hash) = GenerateValidation();
            var voucher = new Voucher
            {
                CampaignId = voucherCampaignId,
                Status = VoucherStatus.InStock,
                ValidationCodeHash = hash,
                OwnerId = ownerId,
                PurchaseDate = DateTime.UtcNow,
            };

            voucher.Id = await _vouchersRepository.CreateAsync(voucher);
            voucher.ShortCode = GenerateShortCodeFromId(voucher.Id);

            await _vouchersRepository.UpdateAsync(voucher, validationCode);

            return BuyVoucherError.None;
        }

        public async Task<RedeemVoucherError> RedeemVoucherAsync(string voucherShortCode, string validationCode)
        {
            var voucher = await _vouchersRepository.GetByShortCodeAsync(voucherShortCode);
            if (voucher == null)
                return RedeemVoucherError.VoucherNotFound;

            var campaign = await _campaignsRepository.GetByIdAsync(voucher.CampaignId);
            if (campaign == null)
                return RedeemVoucherError.VoucherCampaignNotFound;

            if (campaign.State != CampaignState.Published
                || DateTime.UtcNow < campaign.FromDate
                || campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow)
                return RedeemVoucherError.VoucherCampaignNotActive;

            if (voucher.ValidationCode != validationCode)
                return RedeemVoucherError.WrongValidationCode;

            voucher.Status = VoucherStatus.Sold;
            voucher.RedemptionDate = DateTime.UtcNow;

            await _vouchersRepository.UpdateAsync(voucher);

            return RedeemVoucherError.None;
        }

        public async Task<TransferVoucherError> TransferVoucherAsync(
            string voucherShortCode,
            Guid oldOwnerId,
            Guid newOwnerId)
        {
            var voucher = await _vouchersRepository.GetByShortCodeAsync(voucherShortCode);
            if (voucher == null)
                return TransferVoucherError.VoucherNotFound;

            if (voucher.Status != VoucherStatus.InStock)
                return TransferVoucherError.VoucherIsUsed;
            if (voucher.OwnerId != oldOwnerId)
                return TransferVoucherError.NotAnOwner;

            voucher.OwnerId = newOwnerId;
            var (code, hash) = GenerateValidation();
            voucher.ValidationCodeHash = hash;

            await _vouchersRepository.UpdateAsync(voucher, code);

            return TransferVoucherError.None;
        }

        public Task<VoucherWithValidation> GetByShortCodeAsync(string voucherShortCode)
        {
            return _vouchersRepository.GetByShortCodeAsync(voucherShortCode);
        }

        public Task<VouchersPage> GetCampaignVouchersAsync(Guid campaignId, PageInfo pageInfo)
        {
            return _vouchersRepository.GetByCampaignIdAsync(
                campaignId,
                (pageInfo.CurrentPage - 1) * pageInfo.PageSize,
                pageInfo.PageSize);
        }

        public Task<VouchersPage> GetCustomerVouchersAsync(Guid customerId, PageInfo pageInfo)
        {
            return _vouchersRepository.GetByOwnerIdAsync(
                customerId,
                (pageInfo.CurrentPage - 1) * pageInfo.PageSize,
                pageInfo.PageSize);
        }

        public Task<(int publishedCampaingsVouchersCount, int activeCampaingsVouchersCount)> GetPublishedAndActiveCampaignsVouchersCountAsync()
        {
            return _vouchersRepository.GetPublishedAndActiveCampaignsVouchersCountAsync();
        }

        private string GenerateShortCodeFromId(long voucherId)
        {
            var bytes = BitConverter.GetBytes(voucherId);
            return Base32Helper.Encode(bytes);
        }

        private (string, string) GenerateValidation()
        {
            var bytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
            var validationCode = Base32Helper.Encode(bytes);

            var cryptoTransformSha1 = SHA1.Create();
            var sha1 = cryptoTransformSha1.ComputeHash(Encoding.ASCII.GetBytes(validationCode));
            var codeHash = Convert.ToBase64String(sha1);

            return (validationCode, codeHash);
        }
    }
}
