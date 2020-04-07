using System;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Models;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface IFileService
    {
        Task<FileResponse> GetAsync(Guid campaignContentId);

        Task<string> SaveAsync(FileModel file, Guid campaignContentId);

        Task DeleteAsync(Guid campaignContentId);
    }
}
