using System;
using Messages;
using NLog;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    public class RequestMessageHandler : MessageHandler<RcpRequest>
    {
        public RequestMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(RcpRequest message)
        {
            Console.WriteLine("Handling message in RequestMessageHandler which is the SECOND handler");
        }
    }
}