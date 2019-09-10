using System.Collections.Generic;
using RabbitMQ.Client;
using RawRabbit;

namespace PiCalcServer.Services
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly IBusClient busClient;
        private readonly IConnectionFactory connectionFactory;
        private readonly object lockObj = new object();
        private readonly IList<long> runningTasks;

        public SubscriptionHandler(IBusClient busClient, IConnectionFactory connectionFactory)
        {
            this.busClient = busClient;
            this.connectionFactory = connectionFactory;
            runningTasks = new List<long>();
        }

        public void Stop(long id)
        {
            lock (lockObj)
            {
                if (runningTasks.Contains(id))
                {
                    runningTasks.Remove(id);
                }

                var connection = connectionFactory.CreateConnection();
                var model = connection.CreateModel();

                model.BasicReject((ulong) id, false);

            }
        }

        public bool IsStopped(long id)
        {
            lock (lockObj)
            {
                return !runningTasks.Contains(id);
            }
        }

        public void StopAll()
        {
            lock (lockObj)
            {
                foreach (var taskId in runningTasks)
                {
                    Stop(taskId);
                }
            }
        }

        public void Register(long id)
        {
            lock (lockObj)
            {
                runningTasks.Add(id);
            }
        }
    }
}