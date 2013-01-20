using System.Collections.Generic;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class ExchangesConfiguration
    {
        private readonly List<Exchange> _exchanges = new List<Exchange>(); 
        public IEnumerable<Exchange> Exchanges
        {
            get { return _exchanges; }
        } 

        public static ExchangesConfiguration FromCode(params Exchange[] exchanges)
        {
            var conf = new ExchangesConfiguration();
            if(exchanges != null && exchanges.Length > 0)
                conf._exchanges.AddRange(exchanges);

            return conf;
        }

        private ExchangesConfiguration()
        {
            
        }

        public static ExchangesConfiguration FromConfiguration()
        {
            var section = ConfigurationManager.GetSection("ExchangesConfigurationSection") as ExchangeConfigurationSection;
            if(section == null)
                return new ExchangesConfiguration();

            var conf = new ExchangesConfiguration();

            foreach (ExchangeElement exchange in section.Exchanges)
            {
                conf._exchanges.Add(new Exchange
                                        {
                                            Durable = exchange.Durable,
                                            Name = exchange.ExchangeName,
                                            Type = exchange.ExchangeType
                                        });
            }

            return conf;
        }
    }
}