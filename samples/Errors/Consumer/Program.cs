using System;
using System.Threading;
using Messages;
using NLog;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit
                .Initialize(typeof(TestMessageHandler).Assembly)
                .SetupPerformanceCounters()
                .DeclareQueues(QueuesConfiguration.FromCode(new Queue
                                                            {
                                                                Durable = true,
                                                                Name = "SometimesDisconnectedQueue",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 50
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = true,
                                                                Name = "SometimesDisconnectedAsyncQueue",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 50
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = true,
                                                                Name = "SometimesDisconnectedRpcQueue",
                                                                RequiresAck = false,
                                                                IsRcp = true
                                                            }))
                .StartListeningOnDeclaredQueues();
        }
    }

    public class TestMessageHandler : MessageHandler<TestMessageWithPublishDate>
    {
        public TestMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(TestMessageWithPublishDate message)
        {
            var rnd = new Random();
            var sleep = rnd.Next() % 200;
            Console.WriteLine("{0} \t Message with Text: {1} published on: {2} was received on {0}. Sleeping for: {3}", DateTime.Now, message.Text, message.PublishDate, sleep);

            Thread.Sleep(sleep);
        }
    }

    public class TestRcpMessageHandler : MessageHandlerWithResult<TestRpcMessageWithPublishDate>
    {
        public TestRcpMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(TestRpcMessageWithPublishDate message)
        {
            Console.WriteLine("{0} \t RPC Message with Text: {1} published on: {2} was received on {0}", DateTime.Now, message.Text, message.PublishDate);
            return new StringMessage
                       {
                           Content = String.Format("Message Received with Sequence No: {0}", message.SequenceNo)
                       };
        }
    }
}
