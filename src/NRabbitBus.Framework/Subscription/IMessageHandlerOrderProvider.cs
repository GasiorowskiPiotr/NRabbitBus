using System;
using NRabbitBus.Framework.Configuration;

namespace NRabbitBus.Framework.Subscription
{
    public interface IMessageHandlerOrderProvider
    {
        Order GetOrderFor(Type messageType);
        void LoadConfiguration(MessageHandlersOrderConfiguration configuration);
    }
}