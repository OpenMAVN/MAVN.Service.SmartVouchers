using JetBrains.Annotations;

namespace MAVN.Job.SmartVouchers.Settings.JobSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RedisSettings
    {
        public string ConnectionString { set; get; }
    }
}
