using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class RequestMessage : IMessage
    {
        public int SequenceNo { get; set; }
    }
}