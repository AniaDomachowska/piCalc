using System;
using PiCalcContract;
using PiCalcContract.Messages;

namespace PiCalcClient.Publishers
{
    public class PiCalcMessagePublisher : IPiCalcMessagePublisher
    {
        private readonly IMessagePublisher<PiCalcMessage> publisher;

        public PiCalcMessagePublisher(IMessagePublisher<PiCalcMessage> publisher)
        {
            this.publisher = publisher;
        }

        public void PublishBulkMessages(int numberOfTasks, int precision)
        {
            for (var i = 0; i < numberOfTasks; i++)
            {
                var message = new PiCalcMessage
                {
                    Id = DateTime.Now.Ticks + i,
                    Name = $"CalcPi{i}",
                    Precision = precision
                };

                publisher.Publish(message);

                Console.WriteLine($"Published task id: {message.Id}, " +
                                  $"name: {message.Name}, " +
                                  $"precision: {message.Precision}");
            }
        }
    }
}