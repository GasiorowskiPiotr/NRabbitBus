using System;
using System.Collections.Generic;
using Messages;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;

namespace Subscriber2
{
    public class SecondSubscriber : EndpointConfiguration
    {
        public SecondSubscriber()
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
                                                             Name = "Sub2",
                                                             IsRcp = false,
                                                             RequiresAck = false,
                                                             MaxThreads = 10
                                                         });
            
            this.DeclareTheseQueues = queue;
            this.SetupRoutes = RoutesConfiguration.FromCode(new Route(
                                                                "Sub2",
                                                                "PubSubExchange",
                                                                String.Empty));

            this.SetupHandlerOrder = MessageHandlersOrderConfiguration.FromCode(new Order
                                                                                    {
                                                                                        MessageType =
                                                                                            typeof (PublishedMessage),
                                                                                        HandlerOrders =
                                                                                            new List
                                                                                            <MessageHandlerOrder>
                                                                                                {
                                                                                                    new MessageHandlerOrder
                                                                                                        {
                                                                                                            HandlerType
                                                                                                                =
                                                                                                                typeof (
                                                                                                                PublishedMessageSubscriber
                                                                                                                ),
                                                                                                            SequenceNo =
                                                                                                                2
                                                                                                        },
                                                                                                    new MessageHandlerOrder
                                                                                                        {
                                                                                                            HandlerType
                                                                                                                =
                                                                                                                typeof (
                                                                                                                PublishedMessageSubscriber1
                                                                                                                ),
                                                                                                            SequenceNo =
                                                                                                                1
                                                                                                        }
                                                                                                }
                                                                                    });
                                         

            Console.WriteLine("* This Subscriber has a two Handlers for this message. The order of message handling is determined. Please note the messages.");
        }
    }
}