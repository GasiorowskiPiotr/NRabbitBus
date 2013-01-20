namespace NRabbitBus.Framework.MessageProcess
{
    public interface IExtendedMessageSerializer : IMessageSerializer
    {
        void OnBeforeSerialize(ref string objStringForm);
        void OnAfterDeserialize(ref string objStringForm);
    }
}