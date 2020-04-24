using System;
using JetBrains.Annotations;

namespace MAVN.Service.SmartVouchers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RedisSettings
    {
        public string ConnectionString { set; get; }
    }
}
