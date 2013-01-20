using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Configuration
{
    [TestFixture]
    public class Having_RabbitConfigurationProvider
    {
        [TestFixture]
        public class Calling_Get_with_both_Code_and_AppConfig_configuration
        {
            private IRabbitConfigurationProvider _provider;

            [SetUp]
            public void Setup()
            {
                _provider = new RabbitConfigurationProvider(null);
                _provider.Load(RabbitConfiguration.FromCode("aaa", 1, "b", "c"));
            }

            [Test]
            public void Should_always_return_what_is_in_AppConfig()
            {
                var conf = _provider.Get();

                conf.Hostname.Should().Be("localhost");
                conf.Password.Should().Be("guest");
                conf.Username.Should().Be("guest");
                conf.Port.Should().Be(5672);
            }
        }
    }

    [TestFixture]
    public class RabbitConfigurationTest
    {
        [Test]
        public void Should_be_loaded_from_configuration()
        {
            var conf = RabbitConfiguration.FromConfiguration();

            conf.Hostname.Should().Be("localhost");
            conf.Password.Should().Be("guest");
            conf.Username.Should().Be("guest");
            conf.Port.Should().Be(5672);
        }

        [Test]
        public void Should_be_loaded_from_code()
        {
            var conf = RabbitConfiguration.FromCode("localhost", 1234, "piotrek", "piotrek");

            conf.Hostname.Should().Be("localhost");
            conf.Password.Should().Be("piotrek");
            conf.Username.Should().Be("piotrek");
            conf.Port.Should().Be(1234);
        }
    }
}
