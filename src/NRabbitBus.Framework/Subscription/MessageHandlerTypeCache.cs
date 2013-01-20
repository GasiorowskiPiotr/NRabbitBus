using System;
using System.Collections.Generic;
using System.Linq;
using EvilDuck.Framework.Cache;
using EvilDuck.Framework.Container;
using NRabbitBus.Framework.Shared;

namespace NRabbitBus.Framework.Subscription
{
    public class MessageHandlerTypeCache : CustomCache<MessageHandlerTypeCache, Type, IEnumerable<Type>>, IMessageHandlerTypeCache
    {
        protected override IEnumerable<Type> OnMiss(Type messageType)
        {
            IList<Type> resultTypes = new List<Type>();
            resultTypes.Add(typeof(MessageHandler<IMessage>));

            var type = typeof(MessageHandler<>).MakeGenericType(messageType);
            resultTypes.Add(type);

            var type2 = typeof(MessageHandlerWithResult<>).MakeGenericType(messageType);
            resultTypes.Add(type2);

            if (messageType.BaseType != null && messageType.BaseType != typeof(object))
                GetHandlerTypes(messageType.BaseType, resultTypes);

            ValidateTypes(resultTypes);

            return resultTypes;
        }

        private void GetHandlerTypes(Type messageType, IList<Type> resultTypes)
        {
            var type = typeof(MessageHandler<>).MakeGenericType(messageType);
            resultTypes.Add(type);

            var type2 = typeof(MessageHandlerWithResult<>).MakeGenericType(messageType);
            resultTypes.Add(type2);

            if (messageType.BaseType != null && messageType.BaseType != typeof(object))
                GetHandlerTypes(messageType.BaseType, resultTypes);
        }

        private void ValidateTypes(IList<Type> resultTypes)
        {
            var toRemove = resultTypes
                .Where(resultType => ComponentLocator.Current.Get(resultType) == null)
                .ToList();

            foreach (var type in toRemove)
            {
                resultTypes.Remove(type);
            }
        }

        protected override TimeSpan ItemLifeTime
        {
            get { return TimeSpan.FromDays(365); }
        }

        public TimeSpan ItemLifeSpan
        {
            get { return ItemLifeTime; }
        }
    }
}