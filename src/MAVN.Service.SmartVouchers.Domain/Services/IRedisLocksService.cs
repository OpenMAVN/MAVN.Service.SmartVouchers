using System;
using System.Threading.Tasks;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface IRedisLocksService
    {
        Task<bool> TryAcquireLockAsync(string key, string token);

        Task<bool> ReleaseLockAsync(string key, string token);

    }
}
