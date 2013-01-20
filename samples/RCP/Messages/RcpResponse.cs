using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class RcpResponse : IMessage
    {
        public int SequenceNo { get; set; }
    }
}