using System;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    [ConfigurationCollection(typeof(MessageOrderElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "Handler")]
    public class MessageHandlerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MessageHandleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var exchangeElement = element as MessageHandleElement;
            if (exchangeElement == null)
                throw new Exception("element is not of type MessageHandleElement");

            return exchangeElement.TypeName;
        }
    }
}