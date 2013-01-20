using System;
using EvilDuck.Framework.Components;
using NLog;

namespace NRabbitBus.Framework.Subscription
{
    public abstract class MessageHandler<TMessage> : MessageHandler, ISimpleMessageHandler
    {
        protected MessageHandler(Logger logger) : base(logger)
        {
        }

        protected abstract void HandleMessage(TMessage message);

        public override void Handle(object message)
        {
            if (message is TMessage)
            {
                try
                {
                    if (Log.IsDebugEnabled)
                        Log.Debug("Invoking {0} Message Handler", GetType());

                    HandleMessage((TMessage)message);

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
                if(Log.IsWarnEnabled)
                    Log.Warn("MessageHandler invoked but message passed is not of required type ({0}).", typeof(TMessage).FullName);
            }
        }

        protected bool StopOnFailure { get; set; }
    }

    public abstract class MessageHandler : BaseComponent
    {
        protected MessageHandler(Logger logger) : base(logger)
        {
        }

        public abstract void Handle(object message);

        public bool StopProcessing { get; protected set; }
        public object StopProcessingReason { get; protected set; }
    }

    public interface ISimpleMessageHandler
    {
        void Handle(object message);
    }
}