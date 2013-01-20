using System.Linq;
using System.Reflection;
using Autofac;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Logging;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Subscription;
using RabbitMQ.Client;
using Module = Autofac.Module;

namespace NRabbitBus.Framework
{
    public class NRabbitModule : Module
    {
        private readonly bool _scanForFormatters;
        private readonly Assembly[] _assemblies;

        public NRabbitModule(bool scanForFormatters = true, params Assembly[] assemblies)
        {
            _scanForFormatters = scanForFormatters;
            _assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingModule>();

            builder
                .RegisterType<ConnectionFactory>()
                .AsSelf()
                .SingleInstance();

            builder
                .RegisterType<RabbitConnection>()
                .As<IRabbitConnection>()
                .SingleInstance();

            builder
                .Register(c => c.Resolve<IRabbitConnection>().Current.CreateModel())
                .As<IModel>()
                .InstancePerDependency();

            builder
                .RegisterType<MessageFormatter>()
                .As<IMessageFormatter>()
                .InstancePerDependency();

            builder
                .RegisterType<JsonMessageStringifier>()
                .As<IMessageStringifier>()
                .InstancePerDependency();

            builder
                .RegisterType<Utf8MessageSerializer>()
                .As<IMessageSerializer>()
                .InstancePerDependency();

            builder
                .RegisterType<MessagePublisher>()
                .As<IMessagePublisher>()
                .InstancePerDependency();

            builder
                .RegisterType<MessageSubscriber>()
                .As<IMessageSubscriber>()
                .InstancePerDependency();

            builder
                .RegisterType<RpcSubscriber>()
                .As<IMessageSubscriber>()
                .InstancePerDependency();

            builder
                .RegisterType<MessageHandlerOrderProvider>()
                .As<IMessageHandlerOrderProvider>()
                .SingleInstance();

            builder
                .RegisterType<ResponseAwaiter>()
                .As<IResponseAwaiter>()
                .SingleInstance();

            builder
               .RegisterType<RabbitBus>()
               .As<IBus>()
               .InstancePerDependency();

            builder
                .RegisterType<RpcClient>()
                .As<IRpcClient>()
                .InstancePerDependency();

            builder
                .RegisterType<MessageHandlerProvider>()
                .As<IMessageHandlerProvider>()
                .SingleInstance();

            builder
                .RegisterType<MessageHandlerTypeCache>()
                .As<IMessageHandlerTypeCache>()
                .SingleInstance();

            builder
                .RegisterType<RabbitConfigurationProvider>()
                .As<IRabbitConfigurationProvider>()
                .SingleInstance();

            if(_assemblies != null && _assemblies.Length > 0 && _assemblies.All(a => a != null ))
            {
                builder
                    .RegisterAssemblyTypes(_assemblies)
                    .Where(t => t.IsAssignableTo<ISimpleMessageHandler>())
                    .AsSelf()
                    .As<MessageHandler>()
                    .AsClosedTypesOf(typeof(MessageHandler<>))
                    .InstancePerDependency();

                builder
                   .RegisterAssemblyTypes(_assemblies)
                   .Where(t => t.IsAssignableTo<IHandlerWithResult>())
                   .AsSelf()
                   .As<IHandlerWithResult>()
                   .AsClosedTypesOf(typeof(MessageHandlerWithResult<>))
                   .InstancePerDependency();

                builder.RegisterAssemblyModules(_assemblies);

                if (_scanForFormatters)
                {
                    builder
                        .RegisterAssemblyTypes(_assemblies)
                        .Where(t => t.IsAssignableTo<IExtendedMessageFormatter>())
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .InstancePerDependency();

                    builder
                        .RegisterAssemblyTypes(_assemblies)
                        .Where(t => t.IsAssignableTo<IExtendedMessageStringifier>())
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .InstancePerDependency();

                    builder
                        .RegisterAssemblyTypes(_assemblies)
                        .Where(t => t.IsAssignableTo<IExtendedMessageSerializer>())
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .InstancePerDependency();

                    builder
                        .RegisterAssemblyTypes(_assemblies)
                        .Where(t => t.IsAssignableTo<IUnitOfWorkHandler>())
                        .AsImplementedInterfaces()
                        .InstancePerDependency();
                }
            }
        }
    }
}