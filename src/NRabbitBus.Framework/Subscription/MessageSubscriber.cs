using System;
using System.Threading;
using EvilDuck.Framework.Container;
using NLog;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Performance;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.v0_9_1;
using EvilDuck.Framework.Performance;

namespace NRabbitBus.Framework.Subscription
{
    public class MessageSubscriber : Subscriber
    {
        public MessageSubscriber(IMessageHandlerProvider messageHandlerProvider, IRabbitConnection connection, IMessageFormatter messageFormatter, Logger logger) : base(messageHandlerProvider, connection, messageFormatter, logger)
        {
        }

        protected override bool ShouldStartListener(Queue queue)
        {
            return !queue.IsRcp;
        }

        protected override bool ShouldStartDispatcher(Queue queue)
        {
            return !queue.IsRcp;
        }

        protected override void StartQueue(object queueObj)
        {
            try
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Starting queue listened on thread: {0}", Thread.CurrentThread.ManagedThreadId);

                var queue = (Queue)queueObj;
               
                var subscription = new RabbitMQ.Client.MessagePatterns.Subscription(Connection.Current.CreateModel(),
                                                                                        queue.Name, !queue.RequiresAck);
                while (true)
                {
                    var msg = subscription.Next();

                    if(Log.IsInfoEnabled)
                        Log.Info("Received new message {0}", msg.DeliveryTag);

                    Queue.Enqueue(new RabbitMessage(msg.Body, msg.BasicProperties));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception in queue handling", ex);
            }
            
        }

        protected override void DispatchMessage(object msg)
        {
            Performance<RabbitPerformance>.Counter.IncreaseMessagesProcessedPerSecond();
            Performance<RabbitPerformance>.Counter.IncreaseThreadsUsedPerSecond();
            try
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Handling message on thread: {0}", Thread.CurrentThread.ManagedThreadId);

                var rabbitMsg = (RabbitMessage)msg;

                var newScope = ComponentLocator.Current.StartChildScope();
                IMessage result;
                using (newScope)
                {
                    new MessagePipeline(MessageHandlerProvider, MessageFormatter, newScope, rabbitMsg, null).Process(out result);
                }

                var correlationId = rabbitMsg.Properties.CorrelationId;
                var queueName = rabbitMsg.Properties.ReplyTo;

                if (String.IsNullOrEmpty(correlationId) || String.IsNullOrEmpty(queueName))
                    return;

                if (Log.IsInfoEnabled)
                    Log.Info("Message has CorrelationId: {0} and response will be sent to: {1}", correlationId, queueName);

                var channel = Connection.Current.CreateModel();
                IBasicProperties props = new BasicProperties
                {
                    CorrelationId = correlationId
                };

                var payload = MessageFormatter.Format(result);

                if (Log.IsDebugEnabled)
                    Log.Debug("Publishing response.");

                channel.BasicPublish(String.Empty, queueName, props, payload);

                if (Log.IsDebugEnabled)
                    Log.Debug("Published response.");
            }
            catch (Exception ex)
            {
                Log.Error("Exception in message handling", ex);
            }
            finally
            {
                QueueUsedThreads--;
                Performance<RabbitPerformance>.Counter.DecrementThreadsCurrentlyUsed();
            }
        }
    }
}
