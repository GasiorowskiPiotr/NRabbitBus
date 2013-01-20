using System;
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
                .Initialize(typeof (RequestMessageHandler).Assembly)
                .DeclareQueues(QueuesConfiguration.FromCode(new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "AsyncReqResp1",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "AsyncReqResp2",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            },
                                                        new Queue
                                                            {
                                                                Durable = false,
                                                                Name = "AsyncReqResp3",
                                                                RequiresAck = false,
                                                                IsRcp = false,
                                                                MaxThreads = 10
                                                            }))
                .StartListeningOnDeclaredQueues();
        }
    }

    public class RequestMessageHandler : MessageHandlerWithResult<RequestMessage>
    {
        public RequestMessageHandler(Logger logger) : base(logger)
        {
        }


        protected override IMessage HandleMessage(RequestMessage message)
        {
            Console.WriteLine("{0} \t Received message ({1}) with SequenceNo: {2}", DateTime.Now, message.GetType(), message.SequenceNo);
            return new ResponseMessage
                       {
                           SequenceNo = message.SequenceNo
                       };
        }
    }
}
