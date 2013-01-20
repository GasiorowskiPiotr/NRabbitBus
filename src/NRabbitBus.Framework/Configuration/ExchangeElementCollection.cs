using System;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    [ConfigurationCollection(typeof(ExchangeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "Exchange")]
    public class ExchangeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExchangeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var exchangeElement = element as ExchangeElement;
            if(exchangeElement == null)
                throw new Exception("element is not of type ExchangeElement");

            return exchangeElement.ExchangeName;
        }
    }
}