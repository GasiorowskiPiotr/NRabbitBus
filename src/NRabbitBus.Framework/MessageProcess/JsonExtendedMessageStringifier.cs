using NLog;

namespace NRabbitBus.Framework.MessageProcess
{
    public abstract class JsonExtendedMessageStringifier : JsonMessageStringifier, IExtendedMessageStringifier
    {
        protected JsonExtendedMessageStringifier(Logger logger) : base(logger)
        {
        }

        public override string Stringify(object obj)
        {
            if(Log.IsDebugEnabled)
                Log.Debug("OnBeforeStringify calling");

            OnBeforeStringify(obj);

            if (Log.IsDebugEnabled)
                Log.Debug("OnBeforeStringify called");

            var strForm = base.Stringify(obj);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterStringify calling: {0}", strForm);

            OnAfterStringify(ref strForm);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterStringify called: {0}", strForm);

            return strForm;
        }

        public override object Destringify(string objStringForm)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("OnBeforeDestringify calling {0}", objStringForm);

            OnBeforeDestringify(ref objStringForm);

            if (Log.IsDebugEnabled)
                Log.Debug("OnBeforeDestringify called {0}", objStringForm);

            var obj = base.Destringify(objStringForm);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterDestringify calling");

            OnAfterDestringify(obj);

            if (Log.IsDebugEnabled)
                Log.Debug("OnAfterDestringify called");

            return obj;
        }

        public abstract void OnBeforeStringify(object obj);
        public abstract void OnAfterStringify(ref string objStringForm);
        public abstract void OnBeforeDestringify(ref string objStringForm);
        public abstract void OnAfterDestringify(object obj);
    }
}