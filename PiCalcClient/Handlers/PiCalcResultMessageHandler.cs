using System;
using System.Threading.Tasks;
using PiCalcContract;
using PiCalcContract.Messages;
using RawRabbit;

namespace PiCalcClient.Handlers
{
    public class PiCalcResultMessageHandler : BaseSubscriber<PiCalcResultMessage>
    {
        public PiCalcResultMessageHandler(IBusClient busClient) : base(busClient)
        {
        }

        protected override Task HandleMessage(PiCalcResultMessage message)
        {
            Console.WriteLine($"{message.Id} {message.Name}: {message.Pi}");

            return Task.CompletedTask;
        }
    }
}