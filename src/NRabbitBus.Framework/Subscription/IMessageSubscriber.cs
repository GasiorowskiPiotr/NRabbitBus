using NRabbitBus.Framework.Configuration;

namespace NRabbitBus.Framework.Subscription
{
    public interface IMessageSubscriber
    {
        void Subscribe(QueuesConfiguration queuesConfiguration);
        void Unsubscribe();
    }
}