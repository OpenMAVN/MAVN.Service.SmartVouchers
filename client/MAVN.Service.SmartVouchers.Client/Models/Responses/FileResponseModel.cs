namespace MAVN.Service.SmartVouchers.Client.Models.Responses
{
    /// <summary>
    /// Represents a model that contains information for file
    /// </summary>
    public class FileResponseModel
    {
        /// <summary>
        /// Represents the identifier of the file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// File's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File's type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// File's BlobUrl
        /// </summary>
        public string BlobUrl { get; set; }
    }
}
