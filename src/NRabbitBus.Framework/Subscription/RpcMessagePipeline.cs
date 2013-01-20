using Autofac;
using NLog;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    class RpcMessagePipeline : MessagePipelineBase
    {
        public RpcMessagePipeline(IMessageHandlerProvider messageHandlerProvider, ILifetimeScope lifetimeScope, IMessage message, Logger logger)
            : base(messageHandlerProvider, lifetimeScope, message, logger)
        {
        }
    }
}