using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class QueueConfigurationSection : ConfigurationSection
    {
        private const string QueuesPropertyName = "Queues";

        [ConfigurationProperty(QueuesPropertyName)]
        public QueuesElement Queues
        {
            get { return (QueuesElement) this[QueuesPropertyName]; }
        }
    }
}