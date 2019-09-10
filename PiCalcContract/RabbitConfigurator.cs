using RabbitMQ.Client;
using RawRabbit.Configuration;

namespace PiCalcContract
{
    public class RabbitConfigurator
    {
        public static void Configure(RawRabbitConfiguration rabbitJsonConfig)
        {
            var factory = new ConnectionFactory() { };
            factory.UserName = rabbitJsonConfig.Username;
            factory.Password = rabbitJsonConfig.Password;
            factory.VirtualHost = rabbitJsonConfig.VirtualHost;
            factory.HostName = rabbitJsonConfig.Hostnames[0];

            var connection = factory.CreateConnection();

            try
            {
                var model = connection.CreateModel();

                model.ExchangeDeclare("picalccontract.messages", "topic", true);

                model.QueueDeclare("picalcmessage", true, false, false);
                model.QueueDeclare("picalcresultmessage", true, false, false);
                model.QueueDeclare("picalcstopmessage", true, false, false);

                try
                {
                    model.QueueBind("picalcmessage", "picalccontract.messages", "picalcmessage");
                    model.QueueBind("picalcmessage", "picalccontract.messages", "picalcresultmessage");
                    model.QueueBind("picalcmessage", "picalccontract.messages", "picalcstopmessage");
                }
                finally
                {
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}