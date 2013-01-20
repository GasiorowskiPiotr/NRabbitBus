using System;
using System.Reflection;
using EvilDuck.Framework.Components;
using EvilDuck.Framework.Container;
using NLog;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Performance;
using NRabbitBus.Framework.Subscription;
using RabbitMQ.Client;
using EvilDuck.Framework.Performance;

namespace NRabbitBus.Framework
{
    public class Rabbit : BaseComponent
    {
        private static Rabbit _rabbit;
        private QueuesConfiguration _queuesConfiguration;

        public Rabbit() : base(LogManager.GetLogger(typeof(Rabbit).FullName))
        {
        }

        public static void Close()
        {
            var log = LogManager.GetLogger(typeof (Rabbit).FullName);

            if (log.IsInfoEnabled)
                log.Info("Closing Rabbit...");

            if (ComponentLocator.Current != null)
            {
                var subscribers = ComponentLocator.Current.GetAll<IMessageSubscriber>();
                foreach (var messageSubscriber in subscribers)
                {
                    messageSubscriber.Unsubscribe();
                }
            }

            Subscriber.StopReconnecting();
            ComponentLocator.Dispose();
            _rabbit = null;

            if (log.IsInfoEnabled)
                log.Info("Rabbit closed");
        }

        public static Rabbit Initialize(Assembly assembly = null)
        {
            if (_rabbit == null)
                _rabbit = new Rabbit();
            else
                throw new Exception("Rabbit already initialized");

            if(_rabbit.Log.IsInfoEnabled)
                _rabbit.Log.Info("Initializing Rabbit's container...");

            ContainerBootstrap.Initialize(new NRabbitModule(true, assembly));

            if (_rabbit.Log.IsInfoEnabled)
                _rabbit.Log.Info("Rabbit's container initialized.");

            return _rabbit;
        }

        public Rabbit SetupRouting(RoutesConfiguration routesConfiguration)
        {
            if(Log.IsInfoEnabled)
                Log.Info("Setting up Routing...");

            var channel = ComponentLocator.Current.Get<IModel>();

            foreach (var routingEntry in routesConfiguration.Routes)
            {
                if(Log.IsDebugEnabled)
                    Log.Debug("Binding Queue: {0} to Exchange: {1} by Routing Key: {2}", routingEntry.QueueName, routingEntry.ExchangeName, routingEntry.RoutingKey);

                channel.QueueBind(routingEntry.QueueName, routingEntry.ExchangeName, routingEntry.RoutingKey);
            }

            if (Log.IsInfoEnabled)
                Log.Info("Routing set up.");

            return this;
        }

        public Rabbit SetupPerformanceCounters()
        {
            if (Log.IsInfoEnabled)
                Log.Info("Setting up Performance Counters...");

            Performance<RabbitPerformance>.Init();

            if (Log.IsInfoEnabled)
                Log.Info("Performance Counters set up.");

            return this;
        }

        public Rabbit DeclareQueues(QueuesConfiguration queuesConfiguration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Declaring Queues...");

            var channel = ComponentLocator.Current.Get<IModel>();

            foreach (var queue in queuesConfiguration.Queues)
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Declaring Queue: {0}, durable: {1}, exclusive: {2}, autodelete: {3}", queue.Name, queue.Durable, false, false, null);

                channel.QueueDeclare(queue.Name, queue.Durable, false, false, null);
            }

            _queuesConfiguration = queuesConfiguration;

            if (Log.IsInfoEnabled)
                Log.Info("Queues declared.");
            return this;
        }

        public Rabbit DeclareExchanges(ExchangesConfiguration exchangesConfiguration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Declaring Exchanges...");

            var channel = ComponentLocator.Current.Get<IModel>();

            foreach (var exchange in exchangesConfiguration.Exchanges)
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Declaring Exchange: {0}, of type: {1}, durable: {2}", exchange.Name, exchange.Type, exchange.Durable);

                channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable);
            }

            if (Log.IsInfoEnabled)
                Log.Info("Exchanges declared.");

            return this;
        }

        public Rabbit SetMessageHandlerOrder(MessageHandlersOrderConfiguration configuration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Setting MessageHandlers' order from code...");

            var messageHandlerOrderProvider = ComponentLocator.Current.Get<IMessageHandlerOrderProvider>();
            if(messageHandlerOrderProvider != null)
            {
                messageHandlerOrderProvider.LoadConfiguration(configuration);
            }

            if (Log.IsInfoEnabled)
                Log.Info("MessageHandlers' order set.");

            return this;
        }

        public Rabbit SetupConnectionProperties(RabbitConfiguration configuration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Setting up connection properties...");

            var rabbitConfigurationProvider = ComponentLocator.Current.Get<IRabbitConfigurationProvider>();
            if(rabbitConfigurationProvider != null)
            {
                rabbitConfigurationProvider.Load(configuration);
            }

            if (Log.IsInfoEnabled)
                Log.Info("Connection properties set.");

            return this;
        }

        public void StartListeningOnDeclaredQueues()
        {
            if (Log.IsInfoEnabled)
                Log.Info("Starting listening on declared queues...");

            var subscribers = ComponentLocator.Current.GetAll<IMessageSubscriber>();
            foreach (var subscriber in subscribers)
                subscriber.Subscribe(_queuesConfiguration);
        }
    }
}
