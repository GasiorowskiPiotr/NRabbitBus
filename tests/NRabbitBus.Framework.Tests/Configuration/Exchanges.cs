using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Configuration
{
    [TestFixture]
    public class Exchanges
    {
        [Test]
        public void Should_be_loaded_from_configuration()
        {
            var conf = ExchangesConfiguration.FromConfiguration();
            
            conf.Exchanges.Count().Should().Be(2);
            conf.Exchanges.ElementAt(0).Durable.Should().Be(false);
            conf.Exchanges.ElementAt(1).Durable.Should().Be(true);
            conf.Exchanges.ElementAt(0).Name.Should().Be("ex1");
            conf.Exchanges.ElementAt(1).Name.Should().Be("ex2");
            conf.Exchanges.ElementAt(0).Type.Should().Be("direct");
            conf.Exchanges.ElementAt(1).Type.Should().Be("fanout");
        }

        [Test]
        public void Should_be_loaded_from_code()
        {
            var conf = ExchangesConfiguration.FromCode(new Exchange("a", "b", true), new Exchange("c", "d", false));

            conf.Should().NotBeNull();
            conf.Exchanges.Should().NotBeNull();
            conf.Exchanges.Count().Should().Be(2);
            conf.Exchanges.ElementAt(0).Name.Should().Be("a");
            conf.Exchanges.ElementAt(0).Type.Should().Be("b");
            conf.Exchanges.ElementAt(0).Durable.Should().Be(true);
            conf.Exchanges.ElementAt(1).Name.Should().Be("c");
            conf.Exchanges.ElementAt(1).Type.Should().Be("d");
            conf.Exchanges.ElementAt(1).Durable.Should().Be(false);
        }
    }
}
