using System;
using Messages;
using NLog;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    public class RequestReturningMessageHandler : MessageHandlerWithResult<RcpRequest>
    {
        public RequestReturningMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(RcpRequest message)
        {
            Console.WriteLine("Handling message in RequestReturningMessageHandler which is the FIRST handler");
            return new RcpResponse
                       {
                           SequenceNo = message.SequenceNo
                       };
        }
    }
}