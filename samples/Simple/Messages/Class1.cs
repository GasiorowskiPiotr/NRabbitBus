using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class MessageForSimpleQueue : IMessage
    {
        public string Message { get; set; }
    }

    public class MessageForRoutedQueue : IMessage
    {
        public string Message { get; set; }
    }
}
