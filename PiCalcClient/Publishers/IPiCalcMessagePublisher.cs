namespace PiCalcClient.Publishers
{
    public interface IPiCalcMessagePublisher
    {
        void PublishBulkMessages(int numberOfTasks, int precision);
    }
}