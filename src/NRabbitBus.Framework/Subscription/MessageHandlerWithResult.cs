using System;
using NLog;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    public abstract class MessageHandlerWithResult<TMessage> : MessageHandler, IHandlerWithResult
    {
        protected MessageHandlerWithResult(Logger logger) : base(logger)
        {
        }

        public override void Handle(object message)
        {
            if (message is TMessage)
            {
                try
                {
                    if (Log.IsDebugEnabled)
                        Log.Debug("Invoking {0} Message Handler", GetType());

                    Result = HandleMessage((TMessage) message);

                    if (Log.IsDebugEnabled)
                        Log.Debug("Message Handler invoked");
                }
                catch (Exception ex)
                {
                    if (StopOnFailure)
                    {
                        StopProcessing = true;
                        StopProcessingReason = ex;
                    }
                    Log.Error("Error while handling message", ex);
                }

            }
            else
            {
                if (Log.IsWarnEnabled)
                    Log.Warn("MessageHandler invoked but message passed is not of required type ({0}).",
                             typeof (TMessage).FullName);
            }
        }
        protected abstract IMessage HandleMessage(TMessage message);

        protected bool StopOnFailure { get; set; }
        public IMessage Result { get; private set; }
    }
}