using System;
using System.Linq;
using NRabbitBus.Framework.Configuration;

namespace NRabbitBus.Framework.Subscription
{
    public class MessageHandlerOrderProvider : IMessageHandlerOrderProvider
    {
        private volatile MessageHandlersOrderConfiguration _configuration;

        private volatile MessageHandlersOrderConfiguration _codeConfiguration;

        private readonly object _confLock = new object();

        public Order GetOrderFor(Type messageType)
        {
            if(_configuration == null)
            {
                lock (_confLock)
                {
                    if (_configuration == null)
                    {
                        _configuration = MessageHandlersOrderConfiguration.FromConfiguration();
                    }
                }
            }

            Order orderFromConfiguration = null;
            Order orderFromCodeConfiguration = null;

            if(_configuration != null)
                orderFromConfiguration = _configuration.Order.SingleOrDefault(o => o.MessageType == messageType);
            if(_codeConfiguration != null)
                orderFromCodeConfiguration = _codeConfiguration.Order.SingleOrDefault(o => o.MessageType == messageType);

            Order result = null;

            if(orderFromCodeConfiguration != null)
            {
                result = orderFromCodeConfiguration;
            }

            if(orderFromConfiguration != null)
            {
                result = orderFromConfiguration;
            }

            return result;
        }

        public void LoadConfiguration(MessageHandlersOrderConfiguration configuration)
        {
            _codeConfiguration = configuration;
        }
    }
}
