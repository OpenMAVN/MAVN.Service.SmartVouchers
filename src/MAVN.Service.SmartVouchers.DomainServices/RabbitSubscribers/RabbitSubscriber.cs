using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.PaymentManagement.Contract;
using MAVN.Service.SmartVouchers.Domain.Services;

namespace MAVN.Service.SmartVouchers.DomainServices.RabbitSubscribers
{
    public class RabbitSubscriber : JsonRabbitSubscriber<PaymentCompletedEvent>
    {
        private readonly ILog _log;
        private readonly IVouchersService _voucherService;

        public RabbitSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory,
            IVouchersService voucherService)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _log = logFactory.CreateLog(this);
            _voucherService = voucherService;
        }

        protected override async Task ProcessMessageAsync(PaymentCompletedEvent evt)
        {
            await _voucherService.ProcessPaymentRequestAsync(Guid.Parse(evt.PaymentRequestId));

            _log.Info($"Handled {typeof(object).Name}", evt);
        }
    }
}
