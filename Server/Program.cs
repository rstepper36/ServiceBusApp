using NServiceBus;
using NServiceBus.Logging;
using Shared;
using System;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
       
        static async Task Main()
        {
            Console.Title = "Stepper.Server";
            var endpointConfiguration = new EndpointConfiguration("Stepper.Server");
            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendFailedMessagesTo("error");

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

        public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
        {
            static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

            public Task Handle(PlaceOrder message, IMessageHandlerContext context)
            {
                log.Info($"Order for Product:{message.Product} placed with id: {message.Id}");
                log.Info($"Publishing: OrderPlaced for Order Id: {message.Id}");

                var orderPlaced = new OrderPlaced
                {
                    OrderId = message.Id
                };
                return context.Publish(orderPlaced);
            }
        }
    }
}
