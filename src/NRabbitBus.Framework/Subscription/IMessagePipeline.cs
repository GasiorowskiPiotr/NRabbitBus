using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    interface IMessagePipeline
    {
        void Process(out IMessage result);
    }
}