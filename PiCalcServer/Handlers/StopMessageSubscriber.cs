using System;
using System.Threading.Tasks;
using PiCalcContract;
using PiCalcContract.Messages;
using PiCalcServer.Services;
using RawRabbit;

namespace PiCalcServer.Handlers
{
    public class StopMessageSubscriber : BaseSubscriber<PiCalcStopMessage>
    {
        private readonly ISubscriptionHandler subscriptionHandler;

        public StopMessageSubscriber(
            ISubscriptionHandler subscriptionHandler,
            IBusClient busClient) : base(busClient)
        {
            this.subscriptionHandler = subscriptionHandler;
        }

        protected override async Task HandleMessage(PiCalcStopMessage message)
        {
            if (message.Id != null)
            {
                subscriptionHandler.Stop(message.Id.GetValueOrDefault());

                Console.WriteLine($"Stopped message {message.Id}");
            }
            else
            {
                subscriptionHandler.StopAll();

                Console.WriteLine("Stopped all messages");
            }
        }
    }
}