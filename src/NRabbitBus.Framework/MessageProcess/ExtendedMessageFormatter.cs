using NLog;

namespace NRabbitBus.Framework.MessageProcess
{
    public abstract class ExtendedMessageFormatter : MessageFormatter, IExtendedMessageFormatter
    {
        protected ExtendedMessageFormatter(IMessageSerializer messageSerializer, IMessageStringifier messageStringifier, Logger logger) : base(messageSerializer, messageStringifier, logger)
        {
        }

        public override byte[] Format(object obj)
        {
            if(Log.IsDebugEnabled)
                Log.Debug("OnBeforeFormat calling");

            OnBeforeFormat(obj);

            if (Log.IsDebugEnabled)
                Log.Debug("OnBeforeFormat called");

            return base.Format(obj);
        }

        public override object Deformat(byte[] messageBytes)
        {
            var obj = base.Deformat(messageBytes);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterDeformat calling");

            OnAfterDeformat(obj);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterDeformat called");

            return obj;
        }

        public abstract void OnBeforeFormat(object obj);
        public abstract void OnAfterDeformat(object obj);
    }
}