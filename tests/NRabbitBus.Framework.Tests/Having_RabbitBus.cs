using FluentAssertions;
using NRabbitBus.Framework.Shared;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests
{
    [TestFixture]
    public class Having_RabbitBus
    {

        protected IBus Bus;

        [SetUp]
        public void Setup()
        {
            Bus = new RabbitBus(new DummyPublisher(), new DummyRpcClient());
        }

        [Test]
        public void Calling_Request_with_Queue_should_be_called()
        {
            Bus.Request<StringMessage>(new StringMessage(), "a");

            RabbitBusTestContext.RpcClient_Request_with_QueueName_Called.Should().BeTrue();
        }

        [Test]
        public void Calling_Request_with_Exchange_should_be_called()
        {
            Bus.Request<StringMessage>(new StringMessage(), "a", "b", "c");

            RabbitBusTestContext.RpcClient_Request_with_Exchange_Called.Should().BeTrue();
        }

        [Test]
        public void Calling_Publish_to_Queue_should_be_called()
        {
            Bus.Publish(new StringMessage(), "a");

            RabbitBusTestContext.Publisher_Publish_to_Queue_Called.Should().BeTrue();
        }

        [Test]
        public void Calling_Publish_to_Exchange_should_be_called()
        {
            Bus.Publish(new StringMessage(), "a", "b");

            RabbitBusTestContext.Publisher_Publish_to_Exchange_Called.Should().BeTrue();
        }

        [Test]
        public void Calling_Publish_to_Queue_with_Async_Response_should_be_called()
        {
            Bus.Publish(new StringMessage(), "a", message => {});

            RabbitBusTestContext.Publisher_Publish_to_Queue_with_Async_Response_Called.Should().BeTrue();
        }
    }
}