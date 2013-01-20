using System.Text;
using EvilDuck.Framework.Components;
using NLog;

namespace NRabbitBus.Framework.MessageProcess
{
    public class Utf8MessageSerializer : BaseComponent, IMessageSerializer
    {
        public Utf8MessageSerializer(Logger logger) : base(logger)
        {
        }

        public virtual byte[] Serialize(string objStringForm)
        {
            if(Log.IsDebugEnabled)
                Log.Debug("Serializing: {0} as binary", objStringForm);

            return Encoding.UTF8.GetBytes(objStringForm);
        }

        public virtual string Deserialize(byte[] messageBytes)
        {
            var message = Encoding.UTF8.GetString(messageBytes);

            if (Log.IsDebugEnabled)
                Log.Debug("Deserialized message: {0}", message);

            return message;
        }
    }
}