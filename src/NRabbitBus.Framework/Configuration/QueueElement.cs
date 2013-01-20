using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class QueueElement : ConfigurationElement
    {
        private const string NamePropertyName = "name";
        
        [ConfigurationProperty(NamePropertyName)]
        public string Name
        {
            get { return (string) this[NamePropertyName]; }
        }

        private const string AckRequiredPropertyName = "ackRequired";

        [ConfigurationProperty(AckRequiredPropertyName)]
        public bool AckRequired
        {
            get { return (bool) this[AckRequiredPropertyName]; }
        }

        private const string DurablePropertyName = "durable";

        [ConfigurationProperty(DurablePropertyName)]
        public bool Durable
        {
            get { return (bool) this[DurablePropertyName]; }
        }

        private const string TurnOnRcpPropertyName = "isRcp";

        [ConfigurationProperty(TurnOnRcpPropertyName)]
        public bool IsRcp
        {
            get { return (bool) this[TurnOnRcpPropertyName]; }
        }

        private const string MaxThreadsPropertyName = "maxThreads";

        [ConfigurationProperty(MaxThreadsPropertyName)]
        public short MaxThreads
        {
            get { return (short)this[MaxThreadsPropertyName]; }
        }
    }
}