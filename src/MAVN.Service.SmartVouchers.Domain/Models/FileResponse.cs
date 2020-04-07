using System;

namespace MAVN.Service.SmartVouchers.Domain.Models
{
    public class FileResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string BlobUrl { get; set; }
    }
}
