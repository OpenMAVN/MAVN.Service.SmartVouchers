using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.SmartVouchers.Client 
{
    /// <summary>
    /// SmartVouchers client settings.
    /// </summary>
    public class SmartVouchersServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
