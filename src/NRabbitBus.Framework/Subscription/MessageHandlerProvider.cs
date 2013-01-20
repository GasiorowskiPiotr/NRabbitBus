using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using EvilDuck.Framework.Components;
using NLog;

namespace NRabbitBus.Framework.Subscription
{
    public class MessageHandlerProvider : BaseComponent, IMessageHandlerProvider
    {
        private readonly IMessageHandlerTypeCache _messageHandlerTypeCache;

        public MessageHandlerProvider(IMessageHandlerTypeCache messageHandlerTypeCache, Logger logger)
            : base(logger)
        {
            _messageHandlerTypeCache = messageHandlerTypeCache;
        }

        public IEnumerable<Type> GetHandlerTypesFor(Type messageType)
        {
            return _messageHandlerTypeCache.Get(messageType);
        }

        public IEnumerable<MessageHandler> GetHandlersFor(Type messageType, ILifetimeScope lifetimeScope)
        {
            var handlers = GetHandlerTypesFor(messageType)
                .SelectMany(resultType => (IEnumerable<object>)lifetimeScope.Resolve(typeof(IEnumerable<>).MakeGenericType(resultType))
                ).Cast<MessageHandler>();

            var handlersList = handlers.ToList();

            if (Log.IsInfoEnabled)
                Log.Info("Found {0} handlers for message of type {1}", handlersList.Count(), messageType);

            return SortHandlers(handlersList, messageType, lifetimeScope);
        }

        protected IEnumerable<MessageHandler> SortHandlers(IEnumerable<MessageHandler> handlersList, Type messageType, ILifetimeScope lifetimeScope)
        {
            var messageOrderProvider = lifetimeScope.Resolve<IMessageHandlerOrderProvider>();
            var order = messageOrderProvider.GetOrderFor(messageType);
            if (order == null)
                return handlersList;


            var sortedMessageHandlers = order.HandlerOrders.OrderBy(h => h.SequenceNo);
            var resultList = sortedMessageHandlers
                .Select(
                    sortedMessageHandler => handlersList
                                                .SingleOrDefault(
                                                    h => h.GetType() == sortedMessageHandler.HandlerType))
                .Where(handler => handler != null)
                .ToList();

            foreach (var messageHandler in handlersList)
            {
                if (!resultList.Contains(messageHandler))
                    resultList.Add(messageHandler);
            }

            return resultList;
        }
    }
}