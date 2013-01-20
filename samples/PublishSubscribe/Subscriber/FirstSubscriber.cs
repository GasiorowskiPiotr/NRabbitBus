using System;
using System.Collections.Generic;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;

namespace Subscriber
{
    public class FirstSubscriber : EndpointConfiguration
    {
        public FirstSubscriber()
        {
            this.DeclareTheseExchanges = ExchangesConfiguration.FromCode(new Exchange
                                                                         {
                                                                             Durable = false,
                                                                             Name = "PubSubExchange",
                                                                             Type = "fanout"
                                                                         });

            var queue = QueuesConfiguration.FromCode(new Queue
                                                         {
                                                             Durable = false,
                                                             Name = "Sub1",
                                                             IsRcp = false,
                                                             RequiresAck = false,
                                                             MaxThreads = 10
                                                         });
                            

            this.DeclareTheseQueues = queue;
            this.SetupRoutes = RoutesConfiguration.FromCode(new Route(
                                                                "Sub1",
                                                                "PubSubExchange",
                                                                String.Empty));

            Console.WriteLine("* This Subscriber has a UnitOfWork implementation. Please pay attention when UnitOfWork methods are called.");
            Console.WriteLine("* This Subscriber has a two Handlers for this message. The order of message handling is not determined.");

        }
    }
}