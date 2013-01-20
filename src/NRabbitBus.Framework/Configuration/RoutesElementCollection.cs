using System;
using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    [ConfigurationCollection(typeof(RouteElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "Route")]
    public class RoutesElementCollection : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "Route"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RouteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var routeElement = element as RouteElement;
            if (routeElement == null)
                throw new Exception("Element not of type QueueElement");

            return routeElement;
        }
    }
}