namespace NRabbitBus.Framework.MessageProcess
{
    public interface IMessageSerializer
    {
        byte[] Serialize(string objStringForm);
        string Deserialize(byte[] messageBytes);
    }
}