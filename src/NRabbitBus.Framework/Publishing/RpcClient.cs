using EvilDuck.Framework.Components;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace NRabbitBus.Framework.Publishing
{
    public class RpcClient : BaseComponent, IRpcClient
    {
        private readonly IMessageFormatter _messageFormatter;
        private readonly IModel _channel;

        public RpcClient(IMessageFormatter messageFormatter, IModel channel, Logger logger) : base(logger)
        {
            _messageFormatter = messageFormatter;
            _channel = channel;
        }

        public TResponse Request<TResponse>(IMessage message, string queueName) where TResponse : class, IMessage
        {
            if(Log.IsDebugEnabled)
                Log.Debug("Calling Request for Queue: {0} ...", queueName);

            var client = new SimpleRpcClient(_channel, queueName);
            var payload = _messageFormatter.Format(message);

            if (Log.IsDebugEnabled)
                Log.Debug("Request message size is: {0} bytes.", payload.Length);

            var responseBytes = client.Call(payload);

            if(Log.IsDebugEnabled)
                Log.Debug("Received a message.");

            if (Log.IsDebugEnabled)
                Log.Debug("Response message size is: {0} bytes.", responseBytes.Length);

            var response = _messageFormatter.Deformat(responseBytes);

            if (Log.IsDebugEnabled)
                Log.Debug("Calling request completed.");

            return response as TResponse;
        }

        public TResponse Request<TResponse>(IMessage message, string exchangeName, string exchangeType, string routingKey) where TResponse : class, IMessage
        {
            if (Log.IsDebugEnabled)
                Log.Debug("Calling Request for Exchange name: {0} of type {1} with routing key: {2} ...", exchangeName, exchangeType, routingKey);

            var client = new SimpleRpcClient(_channel, exchangeName, exchangeType, routingKey);
            var payload = _messageFormatter.Format(message);

            if (Log.IsDebugEnabled)
                Log.Debug("Request message size is: {0} bytes.", payload.Length);

            var responseBytes = client.Call(payload);

            if (Log.IsDebugEnabled)
                Log.Debug("Received a message.");

            if (Log.IsDebugEnabled)
                Log.Debug("Response message size is: {0} bytes.", responseBytes.Length);

            var response = _messageFormatter.Deformat(responseBytes);

            if (Log.IsDebugEnabled)
                Log.Debug("Calling request completed.");

            return response as TResponse;
        }
    }
}
