using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Lykke.Common;
using Lykke.Common.Log;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Job.SmartVouchers.Services
{
    public class CompletedCampaignsManager : IStartStop
    {
        private readonly ICampaignsService _campaignsService;
        private readonly TimerTrigger _timerTrigger;

        public CompletedCampaignsManager(
            ICampaignsService campaignsService,
            TimeSpan idlePeriod,
            ILogFactory logFactory)
        {
            _campaignsService = campaignsService;
            _timerTrigger = new TimerTrigger(nameof(CompletedCampaignsManager), idlePeriod, logFactory);
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
            await _campaignsService.MarkCampaignsAsCompletedAsync();
        }
    }
}
