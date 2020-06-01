using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.PartnerManagement.Client;
using MAVN.Service.PaymentManagement.Client;
using MAVN.Service.PaymentManagement.Client.Models.Requests;
using MAVN.Service.PaymentManagement.Client.Models.Responses;
using MAVN.Service.SmartVouchers.Contract;
using MAVN.Service.SmartVouchers.Domain.Enums;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    public class VouchersService : IVouchersService
    {
        private const int MaxAttemptsCount = 5;
        private const string SuccessPaymentStatus = "success";
        private const string PendingPaymentStatus = "pending";

        private readonly IPaymentManagementClient _paymentManagementClient;
        private readonly IPartnerManagementClient _partnerManagementClient;
        private readonly IVouchersRepository _vouchersRepository;
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly IPaymentRequestsRepository _paymentRequestsRepository;
        private readonly IRedisLocksService _redisLocksService;
        private readonly IRabbitPublisher<SmartVoucherSoldEvent> _voucherSoldPublisher;
        private readonly IRabbitPublisher<SmartVoucherUsedEvent> _voucherUsedPublisher;
        private readonly ILog _log;
        private readonly TimeSpan _lockTimeOut;

        public VouchersService(
            IPaymentManagementClient paymentManagementClient,
            IPartnerManagementClient partnerManagementClient,
            IVouchersRepository vouchersRepository,
            ICampaignsRepository campaignsRepository,
            IPaymentRequestsRepository paymentRequestsRepository,
            ILogFactory logFactory,
            IRedisLocksService redisLocksService,
            IRabbitPublisher<SmartVoucherSoldEvent> voucherSoldPublisher,
            IRabbitPublisher<SmartVoucherUsedEvent> voucherUsedPublisher,
            TimeSpan lockTimeOut)
        {
            _paymentManagementClient = paymentManagementClient;
            _partnerManagementClient = partnerManagementClient;
            _vouchersRepository = vouchersRepository;
            _campaignsRepository = campaignsRepository;
            _paymentRequestsRepository = paymentRequestsRepository;
            _redisLocksService = redisLocksService;
            _voucherSoldPublisher = voucherSoldPublisher;
            _log = logFactory.CreateLog(this);
            _lockTimeOut = lockTimeOut;
            _voucherUsedPublisher = voucherUsedPublisher;
        }

        public async Task<ProcessingVoucherError> ProcessPaymentRequestAsync(Guid paymentRequestId)
        {
            var voucherShortCode = await _paymentRequestsRepository.GetVoucherShortCodeAsync(paymentRequestId);

            var voucher = await _vouchersRepository.GetByShortCodeAsync(voucherShortCode);
            if (voucher == null)
                return ProcessingVoucherError.VoucherNotFound;

            if (voucher.OwnerId == null)
            {
                _log.Error(message: "Reserved voucher with missing owner", context: voucher);
                throw new InvalidOperationException("Reserved voucher with missing owner");
            }

            var voucherCampaign = await _campaignsRepository.GetByIdAsync(voucher.CampaignId, false);
            if (voucherCampaign == null)
                return ProcessingVoucherError.VoucherCampaignNotFound;

            voucher.Status = VoucherStatus.Sold;
            await _vouchersRepository.UpdateAsync(voucher);

            await PublishVoucherSoldEvent(paymentRequestId, voucherCampaign, voucher);

            return ProcessingVoucherError.None;
        }

        public async Task<VoucherReservationResult> ReserveVoucherAsync(Guid voucherCampaignId, Guid ownerId)
        {
            var campaign = await _campaignsRepository.GetByIdAsync(voucherCampaignId, false);
            if (campaign == null)
                return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.VoucherCampaignNotFound };

            if (campaign.State != CampaignState.Published
                || DateTime.UtcNow < campaign.FromDate
                || campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow)
                return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.VoucherCampaignNotActive };

            if (campaign.VouchersTotalCount <= campaign.BoughtVouchersCount)
                return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.NoAvailableVouchers };



            var voucherPriceIsZero = campaign.VoucherPrice == 0;
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

                var hasAnyReservedVouchers = await _vouchersRepository.AnyReservedVouchersAsync(ownerId);
                if (hasAnyReservedVouchers)
                    return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.CustomerHaveAnotherReservedVoucher };

                var vouchers = await _vouchersRepository.GetByCampaignIdAndStatusAsync(voucherCampaignId, VoucherStatus.InStock);
                Voucher voucher = null;
                if (vouchers.Any())
                {
                    try
                    {
                        voucher = vouchers.First();
                        if (voucherPriceIsZero)
                        {
                            voucher.Status = VoucherStatus.Sold;
                            voucher.OwnerId = ownerId;
                            voucher.PurchaseDate = DateTime.UtcNow;
                            await _vouchersRepository.UpdateAsync(voucher);
                        }
                        else
                        {
                            await _vouchersRepository.ReserveAsync(voucher, ownerId);
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                        await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());
                        return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.NoAvailableVouchers };
                    }
                }
                else
                {
                    var vouchersPage = await _vouchersRepository.GetByCampaignIdAsync(voucherCampaignId, 0, 1);
                    if (vouchersPage.TotalCount >= campaign.VouchersTotalCount)
                    {
                        await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());
                        return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.NoAvailableVouchers };
                    }

                    var validationCode = GenerateValidation();
                    voucher = new Voucher
                    {
                        CampaignId = voucherCampaignId,
                        Status = voucherPriceIsZero ? VoucherStatus.Sold : VoucherStatus.Reserved,
                        OwnerId = ownerId,
                        PurchaseDate = DateTime.UtcNow,
                    };

                    voucher.Id = await _vouchersRepository.CreateAsync(voucher);
                    voucher.ShortCode = GenerateShortCodeFromId(voucher.Id);

                    await _vouchersRepository.UpdateAsync(voucher, validationCode);
                }

                await _redisLocksService.ReleaseLockAsync(voucherCampaignIdStr, ownerId.ToString());

                if (voucherPriceIsZero)
                {
                    await PublishVoucherSoldEvent(null, campaign, voucher);
                    return new VoucherReservationResult
                    {
                        ErrorCode = ProcessingVoucherError.None
                    };
                }

                var paymentRequestResult = await _paymentManagementClient.Api.GeneratePaymentAsync(
                    new PaymentGenerationRequest
                    {
                        CustomerId = ownerId,
                        Amount = campaign.VoucherPrice,
                        Currency = campaign.Currency,
                        PartnerId = campaign.PartnerId,
                        ExternalPaymentEntityId = voucher.ShortCode,
                    });

                if (paymentRequestResult.ErrorCode != PaymentGenerationErrorCode.None)
                {
                    await CancelReservationAsync(voucher.ShortCode);
                    return new VoucherReservationResult
                    {
                        ErrorCode = ProcessingVoucherError.InvalidPartnerPaymentConfiguration,
                    };
                }

                await _paymentRequestsRepository.CreatePaymentRequestAsync(paymentRequestResult.PaymentRequestId, voucher.ShortCode);

                return new VoucherReservationResult
                {
                    ErrorCode = ProcessingVoucherError.None,
                    PaymentUrl = paymentRequestResult.PaymentPageUrl,
                };
            }

            _log.Warning($"Couldn't get a lock for voucher campaign {voucherCampaignId}");

            return new VoucherReservationResult { ErrorCode = ProcessingVoucherError.NoAvailableVouchers };
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
                    await Task.Delay(_lockTimeOut);

                    var result = await CancelReservationAsync(shortCode);
                    if (result != null)
                        break;
                }
            });

            return ProcessingVoucherError.None;
        }

        public async Task<RedeemVoucherError> RedeemVoucherAsync(string voucherShortCode, string validationCode, Guid? sellerCustomerId)
        {
            var voucher = await _vouchersRepository.GetWithValidationByShortCodeAsync(voucherShortCode);
            if (voucher == null)
                return RedeemVoucherError.VoucherNotFound;

            if (voucher.ValidationCode != validationCode)
                return RedeemVoucherError.WrongValidationCode;

            if (voucher.Status != VoucherStatus.Sold)
                return RedeemVoucherError.VoucherIsNotInCorrectStatusToBeRedeemed;

            var campaign = await _campaignsRepository.GetByIdAsync(voucher.CampaignId, false);
            if (campaign == null)
                return RedeemVoucherError.VoucherCampaignNotFound;

            if (!IsCampaignStateValid(campaign) || !IsCampaignDateValid(campaign))
                return RedeemVoucherError.VoucherCampaignNotActive;

            if (sellerCustomerId.HasValue)
            {
                var linkedPartner = await _partnerManagementClient.Linking.GetLinkedPartnerAsync(sellerCustomerId.Value);

                if (!linkedPartner.HasValue)
                    return RedeemVoucherError.SellerCustomerIsNotALinkedPartner;

                if(linkedPartner.Value != campaign.PartnerId)
                    return RedeemVoucherError.SellerCustomerIsNotTheVoucherIssuer;
            }

            voucher.Status = VoucherStatus.Used;
            voucher.RedemptionDate = DateTime.UtcNow;
            voucher.SellerId = sellerCustomerId;

            await _vouchersRepository.UpdateAsync(voucher);
            await PublishVoucherUsedEvent(campaign, voucher);

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
            var validationCode = GenerateValidation();

            await _vouchersRepository.UpdateAsync(voucher, validationCode);

            return TransferVoucherError.None;
        }

        public Task<VoucherWithValidation> GetByShortCodeAsync(string voucherShortCode)
        {
            return _vouchersRepository.GetWithValidationByShortCodeAsync(voucherShortCode);
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

        public async Task ProcessStuckReservedVouchersAsync(TimeSpan generatePaymentTimeoutPeriod, TimeSpan finishPaymentTimeoutPeriod)
        {
            var paymentTimeoutDate = DateTime.UtcNow - generatePaymentTimeoutPeriod;
            var vouchers = await _vouchersRepository.GetReservedVouchersBeforeDateAsync(paymentTimeoutDate);

            foreach (var voucher in vouchers)
            {
                var paymentRequestId = await _paymentRequestsRepository.PaymentRequestExistsAsync(voucher.ShortCode);

                if (paymentRequestId == null)
                {
                    await CancelReservationAsync(voucher.ShortCode);
                    continue;
                }

                var paymentManagementResponse = await _paymentManagementClient.Api.ValidatePaymentAsync(
                    new PaymentValidationRequest
                    {
                        PaymentRequestId = paymentRequestId.Value
                    });

                _log.Info(
                    "Received status for payment request for reserved voucher while processing stuck reserved vouchers",
                    new
                    {
                        VoucherShortCode = voucher.ShortCode,
                        PaymentRequestId = paymentRequestId.Value,
                        PaymentStatus = paymentManagementResponse
                    });

                switch (paymentManagementResponse.ToLower())
                {
                    case SuccessPaymentStatus:
                        await ProcessPaymentRequestAsync(paymentRequestId.Value);
                        continue;
                    case PendingPaymentStatus when voucher.PurchaseDate > DateTime.UtcNow - finishPaymentTimeoutPeriod:
                        continue;
                }

                var paymentManagementError = await _paymentManagementClient.Api.CancelPaymentAsync(
                    new CancelPaymentRequest { PaymentRequestId = paymentRequestId.Value });

                if (paymentManagementError != PaymentCancellationErrorCode.None)
                    _log.Warning("Error when trying to cancel payment in payment management", context: new { paymentRequestId, paymentManagementError });

                await CancelReservationAsync(voucher.ShortCode);
            }
        }

        public async Task MarkVouchersFromFinishedCampaignsAsExpired()
        {
            var finishedCampaignsIds = await _campaignsRepository.GetFinishedCampaignsIdsAsync();
            await _vouchersRepository.SetVouchersFromCampaignsAsExpired(finishedCampaignsIds);
        }

        private bool IsCampaignStateValid(VoucherCampaign campaign)
            => campaign.State != CampaignState.Published;

        private bool IsCampaignDateValid(VoucherCampaign campaign)
            => DateTime.UtcNow < campaign.FromDate
               || (campaign.ToDate.HasValue && campaign.ToDate.Value < DateTime.UtcNow);

        private async Task PublishVoucherSoldEvent(Guid? paymentRequestId, VoucherCampaign voucherCampaign, Voucher voucher)
        {
            await _voucherSoldPublisher.PublishAsync(new SmartVoucherSoldEvent
            {
                Amount = voucherCampaign.VoucherPrice,
                Currency = voucherCampaign.Currency,
                CustomerId = voucher.OwnerId.Value,
                PartnerId = voucherCampaign.PartnerId,
                Timestamp = DateTime.UtcNow,
                CampaignId = voucher.CampaignId,
                VoucherShortCode = voucher.ShortCode,
                PaymentRequestId = paymentRequestId?.ToString(),
            });
        }

        private async Task PublishVoucherUsedEvent(VoucherCampaign voucherCampaign, Voucher voucher)
        {
            await _voucherUsedPublisher.PublishAsync(new SmartVoucherUsedEvent
            {
                CustomerId = voucher.OwnerId.Value,
                Timestamp = DateTime.UtcNow,
                PartnerId = voucherCampaign.PartnerId,
                CampaignId = voucher.CampaignId,
                Amount = voucherCampaign.VoucherPrice,
                Currency = voucherCampaign.Currency,
                LinkedCustomerId = voucher.SellerId,
            });
        }

        private static string GenerateValidation()
        {
            var bytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
            var validationCode = Base32Helper.Encode(bytes);

            return validationCode;
        }

        private string GenerateShortCodeFromId(long voucherId)
        {
            var bytes = BitConverter.GetBytes(voucherId);
            return Base32Helper.Encode(bytes);
        }

        private async Task<ProcessingVoucherError?> CancelReservationAsync(string shortCode)
        {
            var locked = await _redisLocksService.TryAcquireLockAsync(shortCode, shortCode, _lockTimeOut);
            if (!locked)
                return null;

            var voucher = await _vouchersRepository.GetByShortCodeAsync(shortCode);
            if (voucher.Status != VoucherStatus.Reserved)
            {
                await _redisLocksService.ReleaseLockAsync(voucher.ShortCode, voucher.ShortCode);
                return ProcessingVoucherError.VoucherNotFound;
            }

            await _vouchersRepository.CancelReservationAsync(voucher);

            await _redisLocksService.ReleaseLockAsync(voucher.ShortCode, voucher.ShortCode);
            return ProcessingVoucherError.None;
        }
    }
}
