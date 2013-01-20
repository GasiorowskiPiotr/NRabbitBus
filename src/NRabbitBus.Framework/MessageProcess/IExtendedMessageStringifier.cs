namespace NRabbitBus.Framework.MessageProcess
{
    public interface IExtendedMessageStringifier : IMessageStringifier
    {
        void OnBeforeStringify(object obj);
        void OnAfterStringify(ref string objStringForm);
        void OnBeforeDestringify(ref string objStringForm);
        void OnAfterDestringify(object obj);
    }
}