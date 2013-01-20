using System;
using Autofac;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client.Events;

namespace NRabbitBus.Framework.Subscription
{
    public class MessagePipeline : MessagePipelineBase
    {
        public MessagePipeline(IMessageHandlerProvider messageHandlerProvider, IMessageFormatter messageFormatter, ILifetimeScope lifetimeScope, RabbitMessage rabbitMsg, Logger logger)
            : base(messageHandlerProvider, lifetimeScope, GetMessage(messageFormatter, rabbitMsg.Body), logger)
        {
        }

        private static IMessage GetMessage(IMessageFormatter messageFormatter, byte[] messageBytes)
        {
            var message = messageFormatter.Deformat(messageBytes) as IMessage;

            if (message == null)
                throw new Exception("Message must implement IMessage");

            return message;
        }
    }
}