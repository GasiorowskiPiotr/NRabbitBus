using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class RouteElement : ConfigurationElement
    {
        private const string QueuePropertyName = "queue";
        private const string ExchangePropertyName = "exchange";
        private const string RoutingKeyPropertyName = "routingKey";

        [ConfigurationProperty(QueuePropertyName, IsRequired = true)]
        public string QueueName
        {
            get { return (string) this[QueuePropertyName]; }
        }

        [ConfigurationProperty(ExchangePropertyName, IsRequired = true)]
        public string ExchangeName
        {
            get { return (string) this[ExchangePropertyName]; }
        }

        [ConfigurationProperty(RoutingKeyPropertyName, IsRequired = true)]
        public string RoutingKey
        {
            get { return (string) this[RoutingKeyPropertyName]; }
        }
    }
}