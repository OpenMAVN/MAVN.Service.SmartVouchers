using Microsoft.WindowsAzure.Storage.Table;

namespace MAVN.Service.SmartVouchers.AzureRepositories.Entities
{
    public class FileInfoEntity : TableEntity
    {
        public FileInfoEntity()
        {
        }

        public FileInfoEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string CampaignContentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
