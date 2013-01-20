using System.Linq;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Tests.Subscription;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Configuration
{
    [TestFixture]
    public class MessageOrder
    {
        [Test]
        public void Should_be_loaded_from_configuration()
        {
            var conf = MessageHandlersOrderConfiguration.FromConfiguration();

            conf.Order.Count().Should().Be(1);
            conf.Order.ElementAt(0).MessageType.Should().Be(typeof (MyMessage));
            conf.Order.ElementAt(0).HandlerOrders.Count().Should().Be(1);
            conf.Order.ElementAt(0).HandlerOrders.ElementAt(0).SequenceNo.Should().Be(1);
            conf.Order.ElementAt(0).HandlerOrders.ElementAt(0).HandlerType.Should().Be(typeof (MyMessageHandler));
        }

        [Test]
        public void Should_be_loaded_from_code()
        {
            var conf = MessageHandlersOrderConfiguration.FromCode(new Order
                                                                      {
                                                                          MessageType = typeof(MyMessage),
                                                                          HandlerOrders = new[]
                                                                                              {
                                                                                                  new MessageHandlerOrder
                                                                                                      {
                                                                                                          HandlerType = typeof (MyMessageHandler),
                                                                                                          SequenceNo = 6
                                                                                                      }
                                                                                              }
                                                                      });

            conf.Order.Count().Should().Be(1);
            conf.Order.ElementAt(0).MessageType.Should().Be(typeof(MyMessage));
            conf.Order.ElementAt(0).HandlerOrders.Count().Should().Be(1);
            conf.Order.ElementAt(0).HandlerOrders.ElementAt(0).SequenceNo.Should().Be(6);
            conf.Order.ElementAt(0).HandlerOrders.ElementAt(0).HandlerType.Should().Be(typeof(MyMessageHandler));

        }

        [Test]
        public void Should_apply_NullObject_pattern()
        {
            var conf = MessageHandlersOrderConfiguration.Empty();

            conf.Should().NotBeNull();
            conf.Order.Should().NotBeNull();
        }
    }
}
