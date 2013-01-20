using System;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Publishing
{
    public interface IResponseAwaiter
    {
        void EnsureInitialized();
        void RegisterResponseAction(Action<IMessage> responseAction, out IBasicProperties basicProperties);
        void StopAwaiting();

        string QueueName { get; }
        string ResponseListenerThreadId { get; }
        bool IsListenerRunning { get; }

        event EventHandler<EventArgs> AwaiterInitialized;
    }
}