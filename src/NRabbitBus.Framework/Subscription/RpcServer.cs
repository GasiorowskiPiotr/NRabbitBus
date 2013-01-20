using System;
using System.Collections.Concurrent;
using EvilDuck.Framework.Container;
using EvilDuck.Framework.Performance;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Performance;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace NRabbitBus.Framework.Subscription
{
    public class RpcServer : SimpleRpcServer
    {
        private readonly IMessageHandlerProvider _messageHandlerProvider;
        private readonly ConcurrentQueue<RabbitMessage> _queue;

        public RpcServer(IMessageHandlerProvider messageHandlerProvider, RabbitMQ.Client.MessagePatterns.Subscription subscription, ConcurrentQueue<RabbitMessage> queue) : base(subscription)
        {
            _messageHandlerProvider = messageHandlerProvider;
            _queue = queue;
        }

        public override byte[] HandleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            Performance<RabbitPerformance>.Counter.IncreaseMessagesProcessedPerSecond();
            var newScope = ComponentLocator.Current.StartChildScope();
            var messageFormatter = ComponentLocator.Current.Get<IMessageFormatter>();

            var message = messageFormatter.Deformat(body) as IMessage;
            if(message == null)
                throw new Exception("Deserialized message is not of type IMessage.");

            IMessage result;

            using (newScope)
            {
                new RpcMessagePipeline(_messageHandlerProvider, newScope, message, null).Process(out result);
            }

            replyProperties = null;
            return messageFormatter.Format(result);
        }
    }
}