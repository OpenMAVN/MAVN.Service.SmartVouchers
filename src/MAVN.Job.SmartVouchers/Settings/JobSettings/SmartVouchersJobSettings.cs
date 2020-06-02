using System;

namespace MAVN.Job.SmartVouchers.Settings.JobSettings
{
    public class SmartVouchersJobSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings Rabbit { get; set; }

        public RedisSettings Redis { set; get; }

        public TimeSpan VoucherLockTimeOut { get; set; }

        public TimeSpan ExpiredVouchersJobIdlePeriod { get; set; }

        public TimeSpan ReservedVouchersTimeoutJobIdlePeriod { get; set; }

        public TimeSpan GeneratePaymentTimeoutPeriod { get; set; }

        public TimeSpan FinishPaymentTimeoutPeriod { get; set; }

        public TimeSpan CompletedCampaignsJobIdlePeriod { get; set; }
    }
}
