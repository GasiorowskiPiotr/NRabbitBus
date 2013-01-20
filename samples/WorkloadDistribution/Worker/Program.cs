using System;
using System.Threading;
using Messages;
using NLog;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Subscription;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var queues = QueuesConfiguration.FromCode(new Queue
                                                          {
                                                              Durable = false,
                                                              Name = "WorkQueue",
                                                              IsRcp = false,
                                                              RequiresAck = false,
                                                              MaxThreads = 10
                                                          });
                             

            Rabbit
                .Initialize(typeof(Program).Assembly)
                .DeclareQueues(queues)
                .StartListeningOnDeclaredQueues();
        }
    }

    public class WorkMessageHandler : MessageHandler<WaitMsMessage>
    {
        public WorkMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(WaitMsMessage message)
        {
            Console.WriteLine("{0} \t Received work message. Starting to work...", DateTime.Now);

            Thread.Sleep(message.MilisecondsToWait);

            Console.WriteLine("{0} \t Work done!", DateTime.Now);
        }
    }
}
