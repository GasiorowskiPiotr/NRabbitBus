using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NRabbitBus.Framework.Configuration
{
    public class MessageHandlersOrderConfiguration
    {
        private readonly List<Order> _orders = new List<Order>();

        public IEnumerable<Order> Order
        {
            get { return _orders; }
        }

        private MessageHandlersOrderConfiguration()
        {
            
        }

        public static MessageHandlersOrderConfiguration Empty()
        {
            return new MessageHandlersOrderConfiguration();
        }

        public static MessageHandlersOrderConfiguration FromCode(params Order[] orders)
        {
            var conf = new MessageHandlersOrderConfiguration();
            if (orders != null && orders.Length > 0)
                conf._orders.AddRange(orders);

            return conf;
        }

        public static MessageHandlersOrderConfiguration FromConfiguration()
        {
            var section = ConfigurationManager.GetSection("MessageOrderConfigurationSection") as MessageOrderConfigurationSection;
            if (section == null)
                return new MessageHandlersOrderConfiguration();

            var conf = new MessageHandlersOrderConfiguration();

            var orderList = new List<Order>();

            foreach (MessageOrderElement orderElem in section.Orders)
            {
                var order = new Order
                                {
                                    MessageType = Type.GetType(orderElem.MessageType)
                                };

                orderList.Add(order);

                order.HandlerOrders
                    = orderElem
                        .Handlers
                        .OfType<MessageHandleElement>()
                        .Select(elem => new MessageHandlerOrder
                                            {
                                                HandlerType = Type.GetType(elem.TypeName),
                                                SequenceNo = elem.Order
                                            })
                        .ToList();



            }

            conf._orders.AddRange(orderList);
            return conf;
        }
    }

    public class Order
    {
        public Type MessageType { get; set; }

        public IEnumerable<MessageHandlerOrder> HandlerOrders { get; set; }
    }

    public class MessageHandlerOrder
    {
        public Type HandlerType { get; set; }

        public int SequenceNo { get; set; }
    }
}
