using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class ExchangeConfigurationSection : ConfigurationSection
    {
        private const string ExchangesPropertyName = "Exchanges";

        [ConfigurationProperty(ExchangesPropertyName)]
        public ExchangeElementCollection Exchanges
        {
            get { return (ExchangeElementCollection) this[ExchangesPropertyName]; }
        }
    }
}