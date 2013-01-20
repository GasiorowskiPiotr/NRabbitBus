using EvilDuck.Framework.Container;
using FluentAssertions;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests.Subscription
{
    [TestFixture]
    public class When_handling_RPC_call
    {
        private RpcSubscriber _subscriber;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, this.GetType().Assembly));
            ComponentLocator.Current.Get<IModel>().QueueDeclare("TestQueue_RPC", false, false, true, null);
        }

        [Test]
        public void Request_and_Response_should_be_handled()
        {
            _subscriber = new RpcSubscriber(new MessageHandlerProvider(new MessageHandlerTypeCache(), null),  ComponentLocator.Current.Get<IRabbitConnection>(), new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null);
            _subscriber.Subscribe(QueuesConfiguration.FromCode(new Queue()
                                                                   {
                                                                       Name = "TestQueue_RPC",
                                                                       IsRcp = true
                                                                   }));
            var client =
                new RpcClient(
                    new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null),
                    ComponentLocator.Current.Get<IModel>(), null);

            var res = client.Request<MyResultMessage3>(new MyMessage3()
                                                           {
                                                               A = "Piotrek"
                                                           }, "TestQueue_RPC");

            res.B.Should().Be("Piotrek");

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Rabbit.Close();
        }
    }
}