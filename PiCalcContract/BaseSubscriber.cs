using System.Threading.Tasks;
using RawRabbit;
using RawRabbit.Common;

namespace PiCalcContract
{
    public abstract class BaseSubscriber<T>
    {
        private readonly IBusClient busClient;

        protected BaseSubscriber(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        public async Task Subscribe(string queueName)
        {
            busClient.SubscribeAsync<T>(async msg =>
            {
                await HandleMessage(msg);
                return new Ack();
            }).Wait();
        }

        protected abstract Task HandleMessage(T message);
    }
}