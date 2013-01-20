namespace NRabbitBus.Framework.MessageProcess
{
    public interface IMessageFormatter
    {
        byte[] Format(object obj);
        object Deformat(byte[] messageBytes);
    }
}