using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Configuration
{
    [TestFixture]
    public class Queues
    {
        [Test]
        public void Should_be_loaded_from_configuration()
        {
            var conf = QueuesConfiguration.FromConfiguration();

            conf.Queues.Count().Should().Be(1);
            conf.Queues.ElementAt(0).Durable.Should().Be(false);
            conf.Queues.ElementAt(0).RequiresAck.Should().Be(false);
            conf.Queues.ElementAt(0).MaxThreads.Should().Be(12);
            conf.Queues.ElementAt(0).IsRcp.Should().Be(false);
            conf.Queues.ElementAt(0).Name.Should().Be("TestQueue1");
        }

        [Test]
        public void Should_be_loaded_from_code()
        {
            var conf = QueuesConfiguration.FromCode(new Queue("TestQueue1", false, false, false, 12));

            conf.Queues.Count().Should().Be(1);
            conf.Queues.ElementAt(0).Durable.Should().Be(false);
            conf.Queues.ElementAt(0).RequiresAck.Should().Be(false);
            conf.Queues.ElementAt(0).MaxThreads.Should().Be(12);
            conf.Queues.ElementAt(0).IsRcp.Should().Be(false);
            conf.Queues.ElementAt(0).Name.Should().Be("TestQueue1");
        }
    }
}