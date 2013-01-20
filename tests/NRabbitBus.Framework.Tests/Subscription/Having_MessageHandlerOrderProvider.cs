using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Subscription
{
    [TestFixture]
    public class Having_MessageHandlerOrderProvider_configured_from_Code_not_Configuration
    {
        [TestFixture]
        public class When_getting_Order
        {
            private readonly IMessageHandlerOrderProvider _messageHandlerOrderProvider = new MessageHandlerOrderProvider();
            private Order _order;

            [SetUp]
            public void Setup()
            {
                _messageHandlerOrderProvider.LoadConfiguration(
                    MessageHandlersOrderConfiguration.FromCode(
                        new Order
                            {
                                MessageType = typeof (StringMessage),
                                HandlerOrders = new MessageHandlerOrder[0]
                            }));

                _order = _messageHandlerOrderProvider.GetOrderFor(typeof (StringMessage));
            }

            [Test]
            public void It_should_retrieve_order_from_Code()
            {
                _order.MessageType.Should().Be(typeof (StringMessage));
                _order.HandlerOrders.Count().Should().Be(0);
            }
        }
    }

    [TestFixture]
    public class Having_MessageHandlerOrderProvider_configured_from_Configuration_not_Code
    {
        private readonly IMessageHandlerOrderProvider _messageHandlerOrderProvider = new MessageHandlerOrderProvider();
        private Order _order;

        [SetUp]
        public void Setup()
        {
            _order = _messageHandlerOrderProvider.GetOrderFor(typeof(MyMessage));
        }

        [Test]
        public void It_should_retrieve_order_from_Code()
        {
            _order.MessageType.Should().Be(typeof(MyMessage));
            _order.HandlerOrders.Count().Should().Be(1);
        }
    }

    [TestFixture]
    public class Having_MessageHandlerOrderProvider_configured_from_Code_and_Configuration
    {
        private readonly IMessageHandlerOrderProvider _messageHandlerOrderProvider = new MessageHandlerOrderProvider();
        private Order _order;

        [SetUp]
        public void Setup()
        {
            _messageHandlerOrderProvider.LoadConfiguration(
                MessageHandlersOrderConfiguration.FromCode(
                    new Order
                    {
                        MessageType = typeof(MyMessage),
                        HandlerOrders = new MessageHandlerOrder[0]
                    }));

            _order = _messageHandlerOrderProvider.GetOrderFor(typeof(MyMessage));
        }

        [Test]
        public void It_should_retrieve_order_from_Configuration_ignoring_Code()
        {
            _order.MessageType.Should().Be(typeof(MyMessage));
            _order.HandlerOrders.Count().Should().Be(1);
        }
    }
}
