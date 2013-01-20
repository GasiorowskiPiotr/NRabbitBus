using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class RoutingConfigurationSection : ConfigurationSection
    {
        private const string RoutingPropertyName = "Routes";

        [ConfigurationProperty(RoutingPropertyName)]
        public RoutesElementCollection Routes
        {
            get { return (RoutesElementCollection) this[RoutingPropertyName]; }
        }
    }
}