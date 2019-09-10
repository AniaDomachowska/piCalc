using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PiCalcContract;
using PiCalcServer.Handlers;
using PiCalcServer.Services;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using RawRabbit.vNext.Pipe;

namespace PiCalcServer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = InitializeApplication();

            var piCalcMessageSubscriber = serviceProvider.GetService<PiCalcMessageSubscriber>();
            await piCalcMessageSubscriber.Subscribe("picalcrequestmessage");

            var stopMessageSubscriber = serviceProvider.GetService<StopMessageSubscriber>();
            await stopMessageSubscriber.Subscribe("picalcstopmessage");

            Console.ReadLine();
        }

        private static IServiceProvider InitializeApplication()
        {
            var rabbitConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rabbit.json");
            var rabbitJsonConfig =
                JsonConvert.DeserializeObject<RawRabbitConfiguration>(File.ReadAllText(rabbitConfigPath));

            var serviceProvider = new ServiceCollection()
                .AddRawRabbit(new RawRabbitOptions
                {
                    ClientConfiguration = rabbitJsonConfig
                })
                .AddLogging()
                .AddTransient<PiCalcMessageSubscriber>()
                .AddTransient<IPiCalcService, PiCalcService>()
                .AddSingleton<ISubscriptionHandler, SubscriptionHandler>()
                .AddTransient<StopMessageSubscriber>()
                .BuildServiceProvider();

            RabbitConfigurator.Configure(rabbitJsonConfig);

            return serviceProvider;
        }
    }
}