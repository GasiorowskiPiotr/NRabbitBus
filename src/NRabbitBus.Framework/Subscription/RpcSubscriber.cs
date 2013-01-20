using System;
using NLog;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;

namespace NRabbitBus.Framework.Subscription
{
    public class RpcSubscriber : Subscriber
    {
        public RpcSubscriber(IMessageHandlerProvider messageHandlerProvider, IRabbitConnection connection, IMessageFormatter messageFormatter, Logger logger)
            : base(messageHandlerProvider, connection, messageFormatter, logger)
        {
        }

        protected override void StartQueue(object obj)
        {
            var queue = obj as Queue;
            if(queue == null)
                throw new Exception("This is not a queue.");

            var subscribtion = new RabbitMQ.Client.MessagePatterns.Subscription(Connection.Current.CreateModel(), queue.Name);
            new RpcServer(MessageHandlerProvider, subscribtion, Queue).MainLoop();
        }

        protected override void DispatchMessage(object obj)
        {
            
        }

        protected override bool ShouldStartListener(Queue queue)
        {
            return queue.IsRcp;
        }

        protected override bool ShouldStartDispatcher(Queue queue)
        {
            return queue.IsRcp;
        }
    }
}