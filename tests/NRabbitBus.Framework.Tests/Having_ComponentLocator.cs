using System.Linq;
using Autofac;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests
{
    [TestFixture]
    public class Having_ComponentLocator
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, GetType().Assembly));
        }

        [Test]
        public void It_should_create_child_scope_correctly()
        {
            var model = ComponentLocator.Current.Get<IModel>();

            using (var scope = ComponentLocator.Current.StartChildScope())
            {
                var model2 = scope.Resolve<IModel>();

                model.Should().NotBeSameAs(model2);
                model.Should().NotBeNull();
                model2.Should().NotBeNull();
            }
        }

        [Test]
        public void Get_should_work()
        {
            ComponentLocator.Current.Get<IModel>().Should().NotBeNull();
        }

        [Test]
        public void Generic_Get_should_work()
        {
            ComponentLocator.Current.Get(typeof(IModel)).Should().NotBeNull();
        }

        [Test]
        public void GetAll_should_work()
        {
            ComponentLocator.Current.GetAll(typeof (IModel)).Count().Should().Be(1);
        }

        [Test]
        public void Generic_GetAll_should_work()
        {
            ComponentLocator.Current.GetAll<IModel>().Count().Should().Be(1);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ComponentLocator.Dispose();
        }
    }
}
