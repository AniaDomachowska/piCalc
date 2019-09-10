using System;
using System.IO;
using System.Threading.Tasks;
using Args;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PiCalcClient.Handlers;
using PiCalcClient.Publishers;
using PiCalcContract;
using PiCalcContract.Messages;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using RawRabbit.vNext.Pipe;

namespace PiCalcClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = InitializeApplication();

            var piCalcArgs = Configuration.Configure<PiCalcArgs>().CreateAndBind(args);

            if (piCalcArgs.Run)
            {
                var publisher = serviceProvider.GetService<IPiCalcMessagePublisher>();

                publisher.PublishBulkMessages(
                    piCalcArgs.Number.GetValueOrDefault(),
                    piCalcArgs.Precision.GetValueOrDefault());

                var messageHandler = serviceProvider.GetService<PiCalcResultMessageHandler>();

                await messageHandler.Subscribe("PiCalcResult");
            }

            if (piCalcArgs.Stop.HasValue || piCalcArgs.BreakAllTasks)
            {
                var publisher = serviceProvider.GetService<PiCalcStopMessagePublisher>();
                publisher.Publish(piCalcArgs.BreakAllTasks ? null : piCalcArgs.Stop);
                if (piCalcArgs.Stop.HasValue)
                {
                    Console.WriteLine($"Stopped task {piCalcArgs.Stop}");
                }
                else
                {
                    Console.WriteLine("Stopped all tasks");
                }
            }

            Console.ReadLine();
        }

        private static IServiceProvider InitializeApplication()
        {
            var rabbitConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rabbit.json");
            var rabbitJsonConfig =
                JsonConvert.DeserializeObject<RawRabbitConfiguration>(File.ReadAllText(rabbitConfigPath));

            RabbitConfigurator.Configure(rabbitJsonConfig);

            var serviceProvider = new ServiceCollection()
                .AddRawRabbit(new RawRabbitOptions
                {
                    ClientConfiguration = rabbitJsonConfig
                })
                .AddSingleton<PiCalcResultMessageHandler>()
                .AddTransient<IMessagePublisher<PiCalcMessage>, MessagePublisher<PiCalcMessage>>()
                .AddTransient<IMessagePublisher<PiCalcStopMessage>, MessagePublisher<PiCalcStopMessage>>()
                .AddTransient<IPiCalcMessagePublisher, PiCalcMessagePublisher>()
                .AddSingleton<PiCalcStopMessagePublisher>()
                .AddLogging()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}