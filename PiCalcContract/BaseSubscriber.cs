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
            await busClient.SubscribeAsync<T>(async msg =>
            {
                await HandleMessage(msg);
                return new Ack();
            });
        }

        protected abstract Task HandleMessage(T message);
    }
}