using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Publishing
{
    public interface IRpcClient
    {
        TResponse Request<TResponse>(IMessage message, string queueName) where TResponse : class,IMessage;
        TResponse Request<TResponse>(IMessage message, string exchangeName, string exchangeType, string routingKey) where TResponse : class,IMessage;
    }
}