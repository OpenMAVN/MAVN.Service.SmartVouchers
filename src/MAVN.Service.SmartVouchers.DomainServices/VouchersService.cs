using System;
using System.Linq;
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
        private const int MaxAttemptsCount = 5;

        private readonly IVouchersRepository _vouchersRepository;
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly IRedisLocksService _redisLocksService;
        private readonly ILog _log;
        private readonly TimeSpan _lockTimeOut;

        public VouchersService(
            IVouchersRepository vouchersRepository,
            ICampaignsRepository campaignsRepository,
            ILogFactory logFactory,
            IRedisLocksService redisLocksService,
            TimeSpan lockTimeOut)
        {
            _vouchersRepository = vouchersRepository;
            _campaignsRepository = campaignsRepository;
            _redisLocksService = redisLocksService;
            _log = logFactory.CreateLog(this);
            _lockTimeOut = lockTimeOut;
        }

        public async Task<ProcessingVoucherError> BuyVoucherAsync(Guid voucherCampaignId, Guid ownerId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(voucherCampaignId);
            if (campaign == null)
                return ProcessingVoucherError.VoucherCampaignNotFound;

            if (campaign.State != CampaignState.Published
                || DateTime.UtcNow < campaign.FromDate
                || campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow)
                return ProcessingVoucherError.VoucherCampaignNotActive;

            if (campaign.VouchersTotalCount <= campaign.BoughtVouchersCount)
                return ProcessingVoucherError.NoAvailableVouchers;

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

            campaign.BoughtVouchersCount++;
            await _campaignsRepository.UpdateAsync(campaign);

            return ProcessingVoucherError.None;
        }

        public async Task<ProcessingVoucherError> ReserveVoucherAsync(Guid voucherCampaignId, Guid ownerId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(voucherCampaignId);
            if (campaign == null)
                return ProcessingVoucherError.VoucherCampaignNotFound;

            if (campaign.State != CampaignState.Published
                || DateTime.UtcNow < campaign.FromDate
                || campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow)
                return ProcessingVoucherError.VoucherCampaignNotActive;

            if (campaign.VouchersTotalCount <= campaign.BoughtVouchersCount)
                return ProcessingVoucherError.NoAvailableVouchers;

            var voucherCampaignIdStr = voucherCampaignId.ToString();
            for (int i = 0; i < MaxAttemptsCount; ++i)
            {
                var locked = await _redisLocksService.TryAcquireLockAsync(
                    voucherCampaignIdStr,
                    ownerId.ToString(),
                    _lockTimeOut);
                if (!locked)
                {
                    await Task.Delay(_lockTimeOut);
                    continue;
                }

                var vouchers = await _vouchersRepository.GetByCampaignIdAndStatusAsync(voucherCampaignId, VoucherStatus.InStock);
                if (vouchers.Any())
                {
                    try
                    {
                        var voucher = vouchers.FirstOrDefault();
                        voucher.OwnerId = ownerId;
                        voucher.Status = VoucherStatus.Reserved;
                        await _vouchersRepository.ReserveAsync(voucher);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                        await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());
                        return ProcessingVoucherError.NoAvailableVouchers;
                    }
                }
                else
                {
                    var vouchersPage = await _vouchersRepository.GetByCampaignIdAsync(voucherCampaignId, 0, 1);
                    if (vouchersPage.TotalCount >= campaign.VouchersTotalCount)
                    {
                        await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());
                        return ProcessingVoucherError.NoAvailableVouchers;
                    }

                    var (validationCode, hash) = GenerateValidation();
                    var voucher = new Voucher
                    {
                        CampaignId = voucherCampaignId,
                        Status = VoucherStatus.Reserved,
                        ValidationCodeHash = hash,
                        OwnerId = ownerId,
                        PurchaseDate = DateTime.UtcNow,
                    };

                    voucher.Id = await _vouchersRepository.CreateAsync(voucher);
                    voucher.ShortCode = GenerateShortCodeFromId(voucher.Id);

                    await _vouchersRepository.UpdateAsync(voucher, validationCode);
                }

                await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());
                return ProcessingVoucherError.None;
            }

            _log.Warning($"Couldn't get a lock for voucher campaign {voucherCampaignId}");

            return ProcessingVoucherError.NoAvailableVouchers;
        }

        public async Task<ProcessingVoucherError> CancelVoucherReservationAsync(string shortCode)
        {
            var voucher = await _vouchersRepository.GetByShortCodeAsync(shortCode);
            if (voucher == null)
                return ProcessingVoucherError.VoucherNotFound;

            if (voucher.Status != VoucherStatus.Reserved)
                return ProcessingVoucherError.VoucherNotFound;

            var result = await CancelReservationAsync(shortCode);
            if (result != null)
                return result.Value;

            Task.Run(async () =>
            {
                while (true)
                {
                    var result = await CancelReservationAsync(shortCode);
                    if (result != null)
                        break;
                }
            });

            return ProcessingVoucherError.None;
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

        private async Task<ProcessingVoucherError?> CancelReservationAsync(string shortCode)
        {
            var locked = await _redisLocksService.TryAcquireLockAsync(shortCode, shortCode, _lockTimeOut);
            if (!locked)
            {
                await Task.Delay(_lockTimeOut);
                return null;
            }

            var voucher = await _vouchersRepository.GetByShortCodeAsync(shortCode);
            if (voucher.Status != VoucherStatus.Reserved)
            {
                await _redisLocksService.ReleaseLockAsync(voucher.ShortCode, voucher.ShortCode);
                return ProcessingVoucherError.VoucherNotFound;
            }

            voucher.Status = VoucherStatus.InStock;
            await _vouchersRepository.CancelReservationAsync(voucher);

            await _redisLocksService.ReleaseLockAsync(voucher.ShortCode, voucher.ShortCode);
            return ProcessingVoucherError.None;
        }
    }
}
