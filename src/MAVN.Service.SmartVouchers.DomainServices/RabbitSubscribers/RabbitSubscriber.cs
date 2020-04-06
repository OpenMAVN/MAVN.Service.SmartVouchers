using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;

namespace MAVN.Service.SmartVouchers.DomainServices.RabbitSubscribers
{
    // TODO replace object with required message type
    public class RabbitSubscriber : JsonRabbitSubscriber<object>
    {
        private readonly ILog _log;

        public RabbitSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(object message)
        {
            await Task.CompletedTask; // TODO replace this with proper message handling

            _log.Info($"Handled {typeof(object).Name}", message);
        }
    }
}
