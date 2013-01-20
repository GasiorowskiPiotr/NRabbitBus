using System;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Publishing
{
    public interface IMessagePublisher
    {
        void Publish(IMessage message, string queueName);
        void Publish(IMessage message, string exchangeName, string routingKey);
        void Publish(IMessage message, string queueName, Action<IMessage> replyAction);
    }
}