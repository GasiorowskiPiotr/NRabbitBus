namespace NRabbitBus.Framework.Subscription
{
    public interface IUnitOfWorkHandler
    {
        void OnStartProcessing();
        void OnFinishedProcessing();
    }
}
