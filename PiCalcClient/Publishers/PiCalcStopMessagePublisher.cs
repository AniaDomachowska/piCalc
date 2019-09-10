using PiCalcContract;
using PiCalcContract.Messages;

namespace PiCalcClient.Publishers
{
    public class PiCalcStopMessagePublisher
    {
        private readonly IMessagePublisher<PiCalcStopMessage> publisher;

        public PiCalcStopMessagePublisher(IMessagePublisher<PiCalcStopMessage> publisher)
        {
            this.publisher = publisher;
        }

        public void Publish(long? taskId)
        {
            var message = new PiCalcStopMessage
            {
                Id = taskId
            };

            publisher.Publish(message);
        }
    }
}