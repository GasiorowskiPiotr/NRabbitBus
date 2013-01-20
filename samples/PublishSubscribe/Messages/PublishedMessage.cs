using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class PublishedMessage : IMessage
    {
        public string Message { get; set; }
    }
}