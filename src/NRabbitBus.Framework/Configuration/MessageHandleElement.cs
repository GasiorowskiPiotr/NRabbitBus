using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class MessageHandleElement : ConfigurationElement
    {
        private const string OrderName = "order";

        [ConfigurationProperty(OrderName)]
        public int Order
        {
            get { return (int) this[OrderName]; }
        }

        private const string TypeNameName = "type";

        [ConfigurationProperty(TypeNameName)]
        public string TypeName
        {
            get { return (string) this[TypeNameName]; }
        }
    }
}