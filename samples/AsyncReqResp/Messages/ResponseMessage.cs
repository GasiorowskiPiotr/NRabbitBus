using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class ResponseMessage : IMessage
    {
        public int SequenceNo { get; set; }
    }
}