using System;
using System.Threading.Tasks;
using PiCalcContract;
using PiCalcContract.Messages;
using PiCalcServer.Services;
using RawRabbit;

namespace PiCalcServer.Handlers
{
    public class PiCalcMessageSubscriber : BaseSubscriber<PiCalcMessage>
    {
        private readonly IBusClient busClient;
        private readonly IPiCalcService piCalcService;
        private readonly ISubscriptionHandler subscriptionHandler;

        public PiCalcMessageSubscriber(
            IBusClient busClient,
            IPiCalcService piCalcService,
            ISubscriptionHandler subscriptionHandler) : base(busClient)
        {
            this.busClient = busClient;
            this.piCalcService = piCalcService;
            this.subscriptionHandler = subscriptionHandler;
        }

        protected override async Task HandleMessage(PiCalcMessage msg)
        {
            Console.WriteLine($"Started processing task {msg.Id}, " +
                              $"task name: {msg.Name}, " +
                              $"precision: {msg.Precision}...");

            subscriptionHandler.Register(msg.Id);

            var result = piCalcService.Calculate(
                msg.Precision, 
                () => subscriptionHandler.IsStopped(msg.Id));

            var publisher = new MessagePublisher<PiCalcResultMessage>(busClient);

            await publisher.Publish(
                new PiCalcResultMessage
                {
                    Id = msg.Id,
                    Pi = result.ToString()
                });

            Console.WriteLine($"Calculated PI (task Id: {msg.Id}, " +
                              $"task name: {msg.Name}, " +
                              $"precision: {msg.Precision}) : " +
                              $"{result.ToString()}");
        }
    }
}