using System;
using System.Collections.Generic;

namespace NRabbitBus.Framework.Subscription
{
    public interface IMessageHandlerTypeCache
    {
        IEnumerable<Type> Get(Type type);
        TimeSpan ItemLifeSpan { get; }
    }
}