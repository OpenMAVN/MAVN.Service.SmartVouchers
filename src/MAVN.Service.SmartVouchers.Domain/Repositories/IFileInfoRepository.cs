using System;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface IFileInfoRepository
    {
        Task<FileModel> GetAsync(string campaignContentId);

        Task<string> InsertAsync(FileModel model, Guid campaignContentId);

        Task DeleteAsync(string campaignContentId);
    }
}
