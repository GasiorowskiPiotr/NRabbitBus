using System;
using Messages;
using NLog;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            Rabbit
                .Initialize(typeof (Program).Assembly)
                .DeclareQueues(QueuesConfiguration.FromCode(new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "topic1_queue",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "topic2_queue",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "topic3_queue",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            }))
                .DeclareExchanges(ExchangesConfiguration.FromCode(new Exchange
                                                                  {
                                                                      Durable = false,
                                                                      Name = "topic1_exchange",
                                                                      Type = "topic"
                                                                  }))
                .SetupRouting(RoutesConfiguration.FromCode(new Route("topic1_queue", "topic1_exchange",
                                                                                    "piotr.#"),
                                              new Route("topic2_queue", "topic1_exchange",
                                                                                    "piotr.gasiorowski.*"),
                                              new Route("topic3_queue", "topic1_exchange",
                                                                                    "piotr.gasiorowski.programmer")))
                .StartListeningOnDeclaredQueues();
        }
    }

    public class TopicMessageHandler : MessageHandler<TopicMessage>
    {
        public TopicMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(TopicMessage message)
        {
            Console.WriteLine("Received message");
        }
    }
}
