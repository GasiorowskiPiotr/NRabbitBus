namespace NRabbitBus.Framework.MessageProcess
{
    public interface IMessageStringifier
    {
        string Stringify(object obj);
        object Destringify(string objStringForm);
    }
}