using System;
using System.Collections.Generic;
using System.Threading;
using EvilDuck.Framework.Components;
using EvilDuck.Framework.Container;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.v0_9_1;

namespace NRabbitBus.Framework.Publishing
{
    public class ResponseAwaiter : BaseComponent, IResponseAwaiter
    {
        private readonly IMessageFormatter _messageFormatter;


        private string _queueName = String.Empty;
        public string QueueName
        {
            get { return _queueName; }
        }

        private Thread _responseListenerThread;

        public string ResponseListenerThreadId
        {
            get
            {
                if (_responseListenerThread != null)
                    return _responseListenerThread.ManagedThreadId.ToString();
                return String.Empty;
            }
        }

        private bool _isListenerRunning;

        public bool IsListenerRunning
        {
            get { return _isListenerRunning; }
        }

        public event EventHandler<EventArgs> AwaiterInitialized;

        private void OnAwaiterInitialized()
        {
            if (AwaiterInitialized != null)
                AwaiterInitialized(this, EventArgs.Empty);
        }

        private readonly object _lock = new object();

        private readonly Dictionary<string,Action<IMessage>> _responseHandlers = new Dictionary<string, Action<IMessage>>();

        public ResponseAwaiter(IMessageFormatter messageFormatter, Logger logger) : base(logger)
        {
            _messageFormatter = messageFormatter;
        }

        public void EnsureInitialized()
        {
            if(Log.IsDebugEnabled)
                Log.Debug("Ensuring ResponseAwaiter has queue listener");
            if(String.IsNullOrEmpty(_queueName))
            {
                lock (_lock)
                {
                    if(String.IsNullOrEmpty(_queueName))
                    {
                        _queueName = ComponentLocator.Current.Get<IModel>().QueueDeclare().QueueName;

                        if (Log.IsDebugEnabled)
                            Log.Debug("ResponseAwaiter will listen on queue: {0}", _queueName);

                        _responseListenerThread = new Thread(StartAnonymousQueueListener);
                        _responseListenerThread.Start(_queueName);
                    }
                }
            }
        }

        public void RegisterResponseAction(Action<IMessage> responseAction, out IBasicProperties basicProperties)
        {
            var correlationId = Guid.NewGuid().ToString();

            basicProperties = new BasicProperties
                                  {
                                      ReplyTo = _queueName,
                                      CorrelationId = correlationId, 
                                      DeliveryMode = 1
                                  };

            _responseHandlers.Add(correlationId, responseAction);

        }

        public void StopAwaiting()
        {
            _isListenerRunning = false;
            _responseListenerThread.Join(5000);
            _queueName = String.Empty;
            _responseHandlers.Clear();
        }

        private void StartAnonymousQueueListener(object state)
        {
            var queueName = (string)state;

            if (Log.IsInfoEnabled)
                Log.Info("Starting listening on awaiter queue: {0}", queueName);
            
            var model = ComponentLocator.Current.Get<IModel>();
            var subscription = new RabbitMQ.Client.MessagePatterns.Subscription(model, queueName);
            _isListenerRunning = true;
            OnAwaiterInitialized();
            while (_isListenerRunning)
            {
                BasicDeliverEventArgs delivery;
                if(subscription.Next(100, out delivery))
                {
                    if(Log.IsDebugEnabled)
                        Log.Debug("Checking for delivery...");

                    if(delivery == null)
                        continue;

                    if (delivery.BasicProperties == null)
                        continue;

                    var correlationId = delivery.BasicProperties.CorrelationId;

                    if(Log.IsInfoEnabled)
                        Log.Info("Received reply with CorrelationId: {0}");

                    Action<IMessage> resultAction;
                    if(_responseHandlers.TryGetValue(correlationId, out resultAction))
                    {

                        if(Log.IsInfoEnabled)
                            Log.Info("Found reply action for reply with CorrelationId: {0}", correlationId);

                        var message = _messageFormatter.Deformat(delivery.Body) as IMessage;
                        if(message != null)
                        {
                            if(Log.IsInfoEnabled)
                                Log.Info("Executing reply action.");

                            resultAction(message);

                            if (Log.IsInfoEnabled)
                                Log.Info("Executed reply action.");
                        }

                        _responseHandlers.Remove(correlationId);

                    }
                }

            }
        }

    }
}