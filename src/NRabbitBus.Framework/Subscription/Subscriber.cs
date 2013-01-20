using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using EvilDuck.Framework.Components;
using NLog;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Performance;
using EvilDuck.Framework.Performance;

namespace NRabbitBus.Framework.Subscription
{
    public abstract class Subscriber : BaseComponent, IMessageSubscriber
    {
        protected readonly IMessageHandlerProvider MessageHandlerProvider;
        protected readonly IRabbitConnection Connection;
        protected readonly IMessageFormatter MessageFormatter;

        protected readonly ConcurrentQueue<RabbitMessage> Queue = new ConcurrentQueue<RabbitMessage>();

        private readonly IList<Thread> _queueThreads = new List<Thread>();
        private readonly IList<Thread> _dispatcherThread = new List<Thread>(); 

        private bool _isFirstTime = true;
        private static bool _isFinishing;

        private QueuesConfiguration _queuesConfiguration;

        protected short QueueUsedThreads = 0;

        protected Subscriber(IMessageHandlerProvider messageHandlerProvider, IRabbitConnection connection, IMessageFormatter messageFormatter, Logger logger) : base(logger)
        {
            MessageHandlerProvider = messageHandlerProvider;
            Connection = connection;
            Connection.ConnectionMade += ConnectionConnectionMade;
            Connection.ConnectionLost += ConnectionConnectionLost;
            MessageFormatter = messageFormatter;
        }

        public static void StopReconnecting()
        {
            _isFinishing = true;
        }

        private void ConnectionConnectionLost(object sender, EventArgs e)
        {
            bool reconnected = false;
            while (!reconnected && !_isFinishing)
            {
                try
                {
                    var currentCandidate = Connection.Current;
                    if(currentCandidate != null)
                    {
                        if(currentCandidate.CloseReason == null)
                            reconnected = true;
                        else
                        {
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
                
            }
        }

        private void ConnectionConnectionMade(object sender, EventArgs e)
        {
            if(!_isFirstTime)
            {
                foreach (var t in _queueThreads)
                    t.Join(500);

                Subscribe(_queuesConfiguration);
            }
        }

        public void Subscribe(QueuesConfiguration queuesConfiguration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting subscribing to queues");

            foreach (var queue in queuesConfiguration.Queues)
            {
                if(ShouldStartListener(queue))
                {
                    if (Log.IsInfoEnabled)
                        Log.Info("Subscribing to: {0}", queue.Name);

                    var queueThread = new Thread(StartQueue);
                    queueThread.Start(queue);

                    _queueThreads.Add(queueThread);
                }

                if (ShouldStartDispatcher(queue))
                {
                    if (Log.IsInfoEnabled)
                        Log.Info("Starting dispatcher for: {0}", queue.Name);

                    var dispatcherThread = new Thread(StartDispatcher);
                    dispatcherThread.Start(queue);

                    _dispatcherThread.Add(dispatcherThread);
                }
            }

            _isFirstTime = false;
            _queuesConfiguration = queuesConfiguration;

        }

        protected virtual bool ShouldStartListener(Queue queue)
        {
            return true;
        }

        protected virtual bool ShouldStartDispatcher(Queue queue)
        {
            return true;
        }

        public void Unsubscribe()
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting unsubscribing from queues");

            foreach (var thread in _queueThreads)
            {
                thread.Join(TimeSpan.FromSeconds(5));
            }

            if (Log.IsInfoEnabled)
                Log.Info("Unsubscribed");
        }

        protected abstract void StartQueue(object obj);

        protected void StartDispatcher(object obj)
        {
            var queue = obj as Queue;
            if (queue == null)
                throw new Exception("This is not Queue");

            while (true)
            {
                if(!Queue.IsEmpty && QueueUsedThreads < queue.MaxThreads)
                {
                    RabbitMessage message;
                    if(Queue.TryDequeue(out message))
                    {
                        QueueUsedThreads++;
                        Performance<RabbitPerformance>.Counter.IncrementThreadsCurrentlyUsed();
                        ThreadPool.QueueUserWorkItem(DispatchMessage, message);
                    }
                }
                Thread.Sleep(10);
            }
        }

        protected abstract void DispatchMessage(object obj);

        

    }
}