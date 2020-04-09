using Lykke.HttpClientGenerator;

namespace MAVN.Service.SmartVouchers.Client
{
    /// <summary>
    /// SmartVouchers API aggregating interface.
    /// </summary>
    public class SmartVouchersClient : ISmartVouchersClient
    {
        /// <summary>Voucher campaigns Api interface</summary>
        public IVoucherCampaignsApi CampaignsApi { get; }

        /// <summary>Vouchers Api interface</summary>
        public ISmartVouchersApi VouchersApi { get; }

        /// <summary>C-tor</summary>
        public SmartVouchersClient(IHttpClientGenerator httpClientGenerator)
        {
            CampaignsApi = httpClientGenerator.Generate<IVoucherCampaignsApi>();
            VouchersApi = httpClientGenerator.Generate<ISmartVouchersApi>();
        }
    }
}
