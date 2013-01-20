using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class ExchangeElement : ConfigurationElement
    {
        private const string ExchangeNamePropertyName = "name";
        private const string ExchangeTypePropertyName = "type";
        private const string DurablePropertyName = "durable";

        [ConfigurationProperty(ExchangeNamePropertyName)]
        public string ExchangeName
        {
            get { return (string) this[ExchangeNamePropertyName]; }
        }

        [ConfigurationProperty(ExchangeTypePropertyName)]
        public string ExchangeType
        {
            get { return (string) this[ExchangeTypePropertyName]; }
        }

        [ConfigurationProperty(DurablePropertyName)]
        public bool Durable
        {
            get { return (bool) this[DurablePropertyName]; }
        }
    }
}