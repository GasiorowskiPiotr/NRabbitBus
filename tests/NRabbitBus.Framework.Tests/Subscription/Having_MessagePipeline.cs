using System;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;
using RabbitMQ.Client.Framing.v0_9_1;

namespace NRabbitBus.Framework.Tests.Subscription
{
    public class Having_MessagePipeline
    {
        [TestFixture]
        public class Consuming_not_properly_serialized_message
        {
            [Test]
            public void Should_Fail()
            {
                Assert.Throws<Exception>(() =>
                                  {
                                      new MessagePipeline(new MessageHandlerProvider(new MessageHandlerTypeCache(), null), 
                                          new MessageFormatter(new Utf8MessageSerializer(null),
                                                               new JsonMessageStringifier(null), null), null,
                                          new RabbitMessage(new byte[0], new BasicProperties()), null);
                                  });
            }
        }
    }
}