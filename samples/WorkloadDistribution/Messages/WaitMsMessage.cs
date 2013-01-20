using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class WaitMsMessage : IMessage
    {
        public int MilisecondsToWait { get; set; }
    }
}