namespace PiCalcServer.Services
{
    public interface ISubscriptionHandler
    {
        void Stop(long id);
        bool IsStopped(long id);
        void StopAll();
        void Register(long id);
    }
}