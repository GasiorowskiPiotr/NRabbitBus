using System;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework
{
    public class RabbitBus : IBus
    {
        private readonly IMessagePublisher _publisher;
        private readonly IRpcClient _rpcClient;

        public RabbitBus(IMessagePublisher publisher, IRpcClient rpcClient)
        {
            _publisher = publisher;
            _rpcClient = rpcClient;
        }

        public void Publish(IMessage message, string queueName)
        {
            _publisher.Publish(message, queueName);
        }

        public void Publish(IMessage message, string exchangeName, string routingKey)
        {
            _publisher.Publish(message, exchangeName, routingKey);
        }

        public void Publish(IMessage message, string queueName, Action<IMessage> replyAction)
        {
            _publisher.Publish(message, queueName, replyAction);
        }

        public TResponse Request<TResponse>(IMessage message, string queueName) where TResponse : class, IMessage
        {
            return _rpcClient.Request<TResponse>(message, queueName);
        }

        public TResponse Request<TResponse>(IMessage message, string exchangeName, string exchangeType, string routingKey) where TResponse : class, IMessage
        {
            return _rpcClient.Request<TResponse>(message, exchangeName, exchangeType, routingKey);
        }
    }

    public interface IBus
    {
        void Publish(IMessage message, string queueName);
        void Publish(IMessage message, string exchangeName, string routingKey);
        void Publish(IMessage message, string queueName, Action<IMessage> replyAction);

        TResponse Request<TResponse>(IMessage message, string queueName) where TResponse : class, IMessage;
        TResponse Request<TResponse>(IMessage message, string exchangeName, string exchangeType, string routingKey) where TResponse : class, IMessage;
    }
}
