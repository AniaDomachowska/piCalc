using System;
using System.Threading.Tasks;
using RawRabbit;
using RawRabbit.Operations.Publish.Context;

namespace PiCalcContract
{
    public class MessagePublisher<T> : IMessagePublisher<T>
    {
        private readonly IBusClient busClient;

        public MessagePublisher(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        public async Task Publish(T message, Action<IPublishContext> contextAction = null)
        {
            await busClient.PublishAsync(message);
        }
    }
}