using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class MessageOrderElement : ConfigurationElement
    {
        private const string MessageTypeName = "messageType";

        [ConfigurationProperty(MessageTypeName, IsKey = true)]
        public string MessageType
        {
            get { return (string) this[MessageTypeName]; }
        }

        private const string MessageHandlersName = "MessageHandlers";

        [ConfigurationProperty(MessageHandlersName)]
        public MessageHandlerCollection Handlers
        {
            get { return (MessageHandlerCollection) this[MessageHandlersName]; }
        }
    }
}