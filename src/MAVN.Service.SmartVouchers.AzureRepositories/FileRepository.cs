using System;
using System.Threading.Tasks;
using AzureStorage;
using MAVN.Service.SmartVouchers.Domain.Repositories;

namespace MAVN.Service.SmartVouchers.AzureRepositories
{
    public class FileRepository : IFileRepository
    {
        private const string ContainerName = "campaignfiles";

        private readonly IBlobStorage _storage;

        public FileRepository(IBlobStorage storage)
        {
            _storage = storage ??
                throw new ArgumentNullException(nameof(storage));
        }

        public async Task<string> GetBlobUrl(string fileName)
        {
            await _storage.CreateContainerIfNotExistsAsync(ContainerName);

            var hasBlob = await _storage.HasBlobAsync(ContainerName, fileName);

            return hasBlob ?
                _storage.GetBlobUrl(ContainerName, fileName) : null;
        }

        public async Task<string> InsertAsync(byte[] file, string fileName)
        {
            await _storage.CreateContainerIfNotExistsAsync(ContainerName);
            await _storage.SaveBlobAsync(ContainerName, fileName, file);
            return _storage.GetBlobUrl(ContainerName, fileName);
        }

        public async Task DeleteAsync(string id)
        {
            var hasBlob = await _storage.HasBlobAsync(ContainerName, id);

            if (hasBlob)
                await _storage.DelBlobAsync(ContainerName, id);
        }
    }
}
