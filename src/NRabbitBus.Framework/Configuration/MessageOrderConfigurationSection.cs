using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class MessageOrderConfigurationSection : ConfigurationSection
    {
        private const string MessageOrderName = "MessageOrder";

        [ConfigurationProperty(MessageOrderName)]
        public MessageOrderCollection Orders
        {
            get { return (MessageOrderCollection) this[MessageOrderName]; }
        }

    }
}