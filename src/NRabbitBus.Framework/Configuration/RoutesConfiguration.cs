
using System.Collections.Generic;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class RoutesConfiguration
    {
        private readonly List<Route> _entries = new List<Route>();
        
        public IEnumerable<Route> Routes
        {
            get { return _entries; }
        }

        private RoutesConfiguration()
        {
        }

        public static RoutesConfiguration FromConfiguration()
        {
            var section = ConfigurationManager.GetSection("RoutingConfigurationSection") as RoutingConfigurationSection;
            if(section == null)
                return new RoutesConfiguration();

            var routingConfiguration = new RoutesConfiguration();

            foreach (RouteElement route in section.Routes)
            {
                routingConfiguration._entries.Add(new Route(route.QueueName, route.ExchangeName, route.RoutingKey));
            }

            return routingConfiguration;
        }

        public static RoutesConfiguration FromCode(params Route[] routes)
        {
            var conf = new RoutesConfiguration();
            if (routes != null && routes.Length > 0)
                conf._entries.AddRange(routes);

            return conf;
        }
    }
}