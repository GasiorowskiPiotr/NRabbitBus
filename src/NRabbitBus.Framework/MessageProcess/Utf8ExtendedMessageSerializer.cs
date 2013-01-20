using NLog;

namespace NRabbitBus.Framework.MessageProcess
{
    public abstract class Utf8ExtendedMessageSerializer : Utf8MessageSerializer, IExtendedMessageSerializer
    {
        protected Utf8ExtendedMessageSerializer(Logger logger) : base(logger)
        {
        }

        public override byte[] Serialize(string objStringForm)
        {
            if(Log.IsDebugEnabled)
                Log.Debug("Calling on before Serialize on {0}", objStringForm);

            OnBeforeSerialize(ref objStringForm);

            if (Log.IsDebugEnabled)
                Log.Debug("After Calling on before Serialize the message is {0}", objStringForm);

            return base.Serialize(objStringForm);
        }

        public override string Deserialize(byte[] messageBytes)
        {
            var strForm = base.Deserialize(messageBytes);

            if (Log.IsDebugEnabled)
                Log.Debug("Calling on after deserialize on {0}", strForm);

            OnAfterDeserialize(ref strForm);

            if (Log.IsDebugEnabled)
                Log.Debug("After Calling on after deerialize the message is {0}", strForm);

            return strForm;
        }

        public abstract void OnBeforeSerialize(ref string objStringForm);
        public abstract void OnAfterDeserialize(ref string objStringForm);
    }
}