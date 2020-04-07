using System.Threading.Tasks;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<string> GetBlobUrl(string fileName);

        Task<string> InsertAsync(byte[] file, string fileName);

        Task DeleteAsync(string fileName);
    }
}
