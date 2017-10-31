using NServiceBus;
using NServiceBus.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {

        static async Task Main()
        {
            Console.Title = "Stepper.Subscriber";
            var endpointConfiguration = new EndpointConfiguration("Stepper.Subscriber");
            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
        public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
        {
            static ILog log = LogManager.GetLogger<OrderCreatedHandler>();

            public Task Handle(OrderPlaced message, IMessageHandlerContext context)
            {
                log.Info($"Handling: OrderPlaced for Order Id: {message.OrderId}");
                return Task.CompletedTask;
            }
        }
    }
}
