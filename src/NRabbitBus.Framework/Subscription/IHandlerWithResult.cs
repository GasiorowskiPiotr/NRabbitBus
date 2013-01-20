using System;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    public interface IHandlerWithResult
    {
        IMessage Result { get; }
        void Handle(object message);

        bool StopProcessing { get; }
        object StopProcessingReason { get; }
    }
}