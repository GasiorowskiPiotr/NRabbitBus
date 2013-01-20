using System;
using EvilDuck.Framework.Components;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Publishing
{
    public class MessagePublisher : BaseComponent, IMessagePublisher, IDisposable
    {
        private readonly IModel _channel;
        private readonly IMessageFormatter _messageFormatter;
        private readonly IResponseAwaiter _responseAwaiter;

        public MessagePublisher(IModel channel, IMessageFormatter messageFormatter, IResponseAwaiter responseAwaiter, Logger logger) : base(logger)
        {
            _channel = channel;
            _messageFormatter = messageFormatter;
            _responseAwaiter = responseAwaiter;
        }

        public void Publish(IMessage message, string queueName)
        {
            if(Log.IsInfoEnabled)
                Log.Info("Starting publishing message: {0} to queue: {1} ", message, queueName);

            var payload = _messageFormatter.Format(message);

            _channel.BasicPublish(string.Empty, queueName, null, payload);

            if (Log.IsInfoEnabled)
                Log.Info("Message: {0} published to queue: {1} ", message, queueName);
        }

        public void Publish(IMessage message, string exchangeName, string routingKey)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting publishing message: {0} to exchange: {1} with routing key {2}", message, exchangeName, routingKey);

            var payload = _messageFormatter.Format(message);

            _channel.BasicPublish(exchangeName, routingKey, null, payload);

            if (Log.IsInfoEnabled)
                Log.Info("Message: {0} published to exchange: {1} with routing key {2} ", message, exchangeName, routingKey);
        }

        public void Publish(IMessage message, string queueName, Action<IMessage> replyAction)
        {
            if(Log.IsInfoEnabled)
                Log.Info("Starting publishing message: {0} to queue {1}. Asynchronuous response action will be committed.", message, queueName);

            var payload = _messageFormatter.Format(message);

            _responseAwaiter.EnsureInitialized();
            IBasicProperties props;
            _responseAwaiter.RegisterResponseAction(replyAction, out props);

            _channel.BasicPublish(String.Empty, queueName, props, payload);

            if (Log.IsInfoEnabled)
                Log.Info("Message: {0} published to queue: {1}. Asynchronuous response action will be committed.", message, queueName);
        }

        public void Dispose()
        {
            if(Log.IsDebugEnabled)
                Log.Debug("Channel closing.");

            _channel.Close();

            if (Log.IsDebugEnabled)
                Log.Debug("Channel closed. Published disposed.");
        }
    }
}
