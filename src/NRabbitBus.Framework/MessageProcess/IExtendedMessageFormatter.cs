namespace NRabbitBus.Framework.MessageProcess
{
    public interface IExtendedMessageFormatter : IMessageFormatter
    {
        void OnBeforeFormat(object obj);
        void OnAfterDeformat(object obj);
    }
}