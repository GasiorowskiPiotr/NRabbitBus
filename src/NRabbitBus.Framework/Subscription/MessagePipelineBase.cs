using Autofac;
using EvilDuck.Framework.Components;
using NLog;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    public abstract class MessagePipelineBase : BaseComponent, IMessagePipeline
    {
        private readonly IMessageHandlerProvider _messageHandlerProvider;
        protected readonly ILifetimeScope LifetimeScope;
        protected IMessage Message;

        protected MessagePipelineBase(IMessageHandlerProvider messageHandlerProvider, ILifetimeScope lifetimeScope, IMessage message, Logger logger)
            : base(logger)
        {
            _messageHandlerProvider = messageHandlerProvider;
            LifetimeScope = lifetimeScope;
            Message = message;
        }

        public void Process(out IMessage result)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting processing of message");

            var handlers = _messageHandlerProvider.GetHandlersFor(Message.GetType(), LifetimeScope);
            
            IUnitOfWorkHandler unitOfWorkHandler;
            LifetimeScope.TryResolve(out unitOfWorkHandler);

            if (unitOfWorkHandler != null)
                unitOfWorkHandler.OnStartProcessing();

            IMessage res = null;

            foreach (var handler in handlers)
            {
                handler.Handle(Message);

                var handlerWithResult = handler as IHandlerWithResult;
                if (handlerWithResult != null)
                    res = handlerWithResult.Result;

                if (handler.StopProcessing)
                {
                    if (Log.IsInfoEnabled)
                        Log.Info("Message processing stopped");

                    if (handler.StopProcessingReason != null)
                        if (Log.IsWarnEnabled)
                            Log.Warn("Message processing stopped because of: ", handler.StopProcessingReason);

                    break;
                }
            }
            
            result = res;

            if (unitOfWorkHandler != null)
                unitOfWorkHandler.OnFinishedProcessing();

            if (Log.IsInfoEnabled)
                Log.Info("Message processed");            
        }
    }
}