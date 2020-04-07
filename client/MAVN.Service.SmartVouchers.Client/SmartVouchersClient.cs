using Lykke.HttpClientGenerator;

namespace MAVN.Service.SmartVouchers.Client
{
    /// <summary>
    /// SmartVouchers API aggregating interface.
    /// </summary>
    public class SmartVouchersClient : ISmartVouchersClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to SmartVouchers Api.</summary>
        public IVoucherCampaignsApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public SmartVouchersClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IVoucherCampaignsApi>();
        }
    }
}
