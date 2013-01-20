using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class RcpRequest : IMessage
    {
        public int SequenceNo { get; set; }
    }
}