using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Lykke.Common;
using Lykke.Common.Log;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Job.SmartVouchers.Services
{
    public class ExpiredVouchersManager : IStartStop
    {
        private readonly IVouchersService _vouchersService;
        private readonly TimerTrigger _timerTrigger;

        public ExpiredVouchersManager(
            IVouchersService vouchersService,
            TimeSpan idlePeriod,
            ILogFactory logFactory)
        {
            _vouchersService = vouchersService;
            _timerTrigger = new TimerTrigger(nameof(ExpiredVouchersManager), idlePeriod, logFactory);
            _timerTrigger.Triggered += Execute;
        }

        public void Start()
        {
            _timerTrigger.Start();
        }

        public void Stop()
        {
            _timerTrigger.Stop();
        }

        public void Dispose()
        {
            _timerTrigger.Stop();
            _timerTrigger.Dispose();
        }

        private async Task Execute(ITimerTrigger timer, TimerTriggeredHandlerArgs args, CancellationToken cancellationToken)
        {
            await _vouchersService.MarkVouchersFromExpiredCampaignsAsExpired();
        }
    }
}
