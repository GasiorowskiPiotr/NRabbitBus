using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EvilDuck.Framework.Components;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NLog;
using NRabbitBus.Framework.Logging;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Logging
{
    [TestFixture]
    public class Having_Logging_module_in_container
    {
        [Test]
        public void It_should_properly_resolve_logger()
        {
            ContainerBootstrap.Initialize(new SampleModuleWithLogging());

            var component = ComponentLocator.Current.Get<SampleComponent>();
            
            component.Logger.Should().NotBeNull();
            component.Logger.Name.Should().Be(typeof (SampleComponent).FullName);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ContainerBootstrap.Close();
        }
    }

    public class SampleComponent : BaseComponent
    {
        public SampleComponent(Logger logger) : base(logger)
        {
        }

        public Logger Logger
        {
            get { return Log; }
        }
    }

    public class SampleModuleWithLogging : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingModule>();
            builder.RegisterType<SampleComponent>().AsSelf().InstancePerDependency();
        }
    }
}
