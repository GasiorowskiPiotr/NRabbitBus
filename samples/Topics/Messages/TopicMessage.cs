using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class TopicMessage : IMessage
    {
        public string Text { get; set; }
    }
}