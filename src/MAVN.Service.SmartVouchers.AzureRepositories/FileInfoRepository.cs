using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using MAVN.Service.SmartVouchers.AzureRepositories.Entities;
using MAVN.Service.SmartVouchers.Domain.Models;
using MAVN.Service.SmartVouchers.Domain.Repositories;

namespace MAVN.Service.SmartVouchers.AzureRepositories
{
    public class FileInfoRepository : IFileInfoRepository
    {
        private readonly INoSQLTableStorage<FileInfoEntity> _storage;
        private readonly IMapper _mapper;

        private static string GetPartitionKey(string campaignId) => campaignId;

        private static string GetRowKey() => Guid.NewGuid().ToString("D");

        public FileInfoRepository(
            INoSQLTableStorage<FileInfoEntity> storage,
            IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<FileModel> GetAsync(string campaignContentId)
        {
            var entity = await _storage.GetDataAsync(GetPartitionKey(campaignContentId));

            return _mapper.Map<FileModel>(entity.FirstOrDefault());
        }

        public async Task<string> InsertAsync(FileModel fileInfo, Guid campaignContentId)
        {
            var entity = new FileInfoEntity(GetPartitionKey(campaignContentId.ToString()), GetRowKey());

            _mapper.Map(fileInfo, entity);

            await _storage.InsertAsync(entity);

            return entity.RowKey;
        }

        public async Task DeleteAsync(string campaignContentId)
        {
            var entities = await _storage.GetDataAsync(GetPartitionKey(campaignContentId));

            await _storage.DeleteAsync(entities.FirstOrDefault());
        }
    }
}
