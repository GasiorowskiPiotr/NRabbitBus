using System.Collections.Generic;
using Messages;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit
                .Initialize(typeof(RequestMessageHandler).Assembly)
                .DeclareQueues(QueuesConfiguration.FromCode(new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "RcpQueue1",
                                                                RequiresAck = false,
                                                                IsRcp = true
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "RcpQueue2",
                                                                RequiresAck = false,
                                                                IsRcp = true
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "RcpQueue3",
                                                                RequiresAck = false,
                                                                IsRcp = true
                                                            }))
                .SetMessageHandlerOrder(MessageHandlersOrderConfiguration.FromCode(new Order
                                                                    {
                                                                        MessageType = typeof(RcpRequest),
                                                                        HandlerOrders = new List<MessageHandlerOrder>
                                                                                            {
                                                                                                new MessageHandlerOrder
                                                                                                    {
                                                                                                        HandlerType = typeof(RequestMessageHandler),
                                                                                                        SequenceNo = 2
                                                                                                    },
                                                                                                new MessageHandlerOrder
                                                                                                    {
                                                                                                        HandlerType = typeof(RequestReturningMessageHandler),
                                                                                                        SequenceNo = 1
                                                                                                    }
                                                                                            }
                                                                    }))
                .StartListeningOnDeclaredQueues();
        }
    }
}
