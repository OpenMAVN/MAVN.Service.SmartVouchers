using System;
using System.Threading.Tasks;
using MAVN.Service.SmartVouchers.Domain.Services;
using StackExchange.Redis;

namespace MAVN.Service.SmartVouchers.DomainServices
{
    public class RedisLocksService : IRedisLocksService
    {
        private readonly IDatabase _db;

        public RedisLocksService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public Task<bool> TryAcquireLockAsync(string key, string token, TimeSpan ttl)
        {
            return _db.LockTakeAsync(key, token, ttl);
        }

        public Task<bool> ReleaseLockAsync(string key, string token)
        {
            return _db.LockReleaseAsync(key, token);
        }
    }
}
