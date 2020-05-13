using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Common;
using Lykke.Common.Log;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Job.SmartVouchers.Services
{
    public class ReservedVouchersTimeoutManager : IStartStop
    {
        private readonly IVouchersService _vouchersService;
        private readonly TimeSpan _generatePaymentTimeout;
        private readonly TimeSpan _finishPaymentTimeout;
        private readonly TimerTrigger _timerTrigger;

        public ReservedVouchersTimeoutManager(
            IVouchersService vouchersService,
            TimeSpan idlePeriod,
            TimeSpan generatePaymentTimeout,
            TimeSpan finishPaymentTimeout,
            ILogFactory logFactory)
        {
            _vouchersService = vouchersService;
            _generatePaymentTimeout = generatePaymentTimeout;
            _finishPaymentTimeout = finishPaymentTimeout;
            _timerTrigger = new TimerTrigger(nameof(ReservedVouchersTimeoutManager), idlePeriod, logFactory);
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
            await _vouchersService.ProcessStuckReservedVouchersAsync(_generatePaymentTimeout, _finishPaymentTimeout);
        }
    }
}
