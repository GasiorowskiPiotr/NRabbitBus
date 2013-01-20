using System;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    [ConfigurationCollection(typeof(MessageOrderElement), 
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, 
        AddItemName = "Order")]
    public class MessageOrderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MessageOrderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var exchangeElement = element as MessageOrderElement;
            if (exchangeElement == null)
                throw new Exception("element is not of type MessageOrderElement");

            return exchangeElement.MessageType;
        }
    }
}