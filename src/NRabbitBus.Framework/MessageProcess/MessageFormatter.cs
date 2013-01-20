using EvilDuck.Framework.Components;
using NLog;

namespace NRabbitBus.Framework.MessageProcess
{
    public class MessageFormatter : BaseComponent, IMessageFormatter
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IMessageStringifier _messageStringifier;

        public MessageFormatter(IMessageSerializer messageSerializer, IMessageStringifier messageStringifier, Logger logger) : base(logger)
        {
            _messageSerializer = messageSerializer;
            _messageStringifier = messageStringifier;
        }

        public virtual byte[] Format(object obj)
        {
            if(Log.IsInfoEnabled)
                Log.Info("Starting formatting of object {0}", obj);

            var extendedMessageStringifier = _messageStringifier as IExtendedMessageStringifier;
            if(extendedMessageStringifier != null)
                extendedMessageStringifier.OnBeforeStringify(obj);

            var stringForm = _messageStringifier.Stringify(obj);

            if(extendedMessageStringifier != null)
                extendedMessageStringifier.OnAfterStringify(ref stringForm);

            var extendedMessageSerializer = _messageSerializer as IExtendedMessageSerializer;
            if(extendedMessageSerializer != null)
                extendedMessageSerializer.OnBeforeSerialize(ref stringForm);

            var data = _messageSerializer.Serialize(stringForm);

            if (Log.IsInfoEnabled)
                Log.Info("Object formatted");

            return data;
        }

        public virtual object Deformat(byte[] messageBytes)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting deformatting");

            var stringForm = _messageSerializer.Deserialize(messageBytes);

            var extendedMessageSerializer = _messageSerializer as IExtendedMessageSerializer;
            if(extendedMessageSerializer != null)
                extendedMessageSerializer.OnAfterDeserialize(ref stringForm);

            var extendedMessageStringifier = _messageStringifier as IExtendedMessageStringifier;
            if(extendedMessageStringifier != null)
                extendedMessageStringifier.OnBeforeDestringify(ref stringForm);

            var obj = _messageStringifier.Destringify(stringForm);

            if(extendedMessageStringifier != null)
                extendedMessageStringifier.OnAfterDestringify(obj);

            if (Log.IsInfoEnabled)
                Log.Info("Object deformatted");

            return obj;
        }
    }
}
