using System;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    [ConfigurationCollection(typeof(QueueElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "Queue")]
    public class QueuesElement : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "Queue"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var queue = element as QueueElement;
            if(queue == null)
                throw new Exception("Element not of type QueueElement");

            return queue.Name;
        }
    }
}