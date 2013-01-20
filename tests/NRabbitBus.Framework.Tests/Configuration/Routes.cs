using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Configuration
{
    [TestFixture]
    public class Routes
    {
        [Test]
        public void Should_be_loaded_from_configuration()
        {
            var conf = RoutesConfiguration.FromConfiguration();

            conf.Routes.Count().Should().Be(2);
            conf.Routes.ElementAt(0).QueueName.Should().Be("TestQueue1");
            conf.Routes.ElementAt(1).QueueName.Should().Be("TestQueue2");
            conf.Routes.ElementAt(0).ExchangeName.Should().Be("ex1");
            conf.Routes.ElementAt(1).ExchangeName.Should().Be("ex2");
            conf.Routes.ElementAt(0).RoutingKey.Should().Be("a1b1");
            conf.Routes.ElementAt(1).RoutingKey.Should().Be("a1b1");
        }

        [Test]
        public void Should_be_loaded_from_code()
        {
            var conf = RoutesConfiguration.FromCode(
                new Route("TestQueue1", "ex1", "a1b1"),
                new Route("TestQueue2", "ex2", "a1b1"));

            conf.Routes.Count().Should().Be(2);
            conf.Routes.ElementAt(0).QueueName.Should().Be("TestQueue1");
            conf.Routes.ElementAt(1).QueueName.Should().Be("TestQueue2");
            conf.Routes.ElementAt(0).ExchangeName.Should().Be("ex1");
            conf.Routes.ElementAt(1).ExchangeName.Should().Be("ex2");
            conf.Routes.ElementAt(0).RoutingKey.Should().Be("a1b1");
            conf.Routes.ElementAt(1).RoutingKey.Should().Be("a1b1");
        }
    }
}