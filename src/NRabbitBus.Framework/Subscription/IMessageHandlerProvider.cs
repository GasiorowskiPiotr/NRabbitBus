using System;
using System.Collections.Generic;
using Autofac;

namespace NRabbitBus.Framework.Subscription
{
    public interface IMessageHandlerProvider
    {
        IEnumerable<Type> GetHandlerTypesFor(Type messageType);
        IEnumerable<MessageHandler> GetHandlersFor(Type messageType, ILifetimeScope lifetimeScope);
    }
}