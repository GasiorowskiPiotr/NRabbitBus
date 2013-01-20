namespace NRabbitBus.Framework.Shared
{
    public class StringMessage : IMessage
    {
        public string Content { get; set; }
    }
}