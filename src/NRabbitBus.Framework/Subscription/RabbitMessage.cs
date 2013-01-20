using RabbitMQ.Client;

namespace NRabbitBus.Framework.Subscription
{
    public class RabbitMessage
    {
        public RabbitMessage(byte[] body, IBasicProperties properties)
        {
            Body = body;
            Properties = properties;
        }

        public byte[] Body { get; set; }
        public IBasicProperties Properties { get; set; }
    }
}