namespace NRabbitBus.Framework.Configuration
{
    public class Route
    {
        public Route(string queueName, string exchangeName, string routingKey)
        {
            QueueName = queueName;
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
        }

        public Route()
        {
        }

        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
}