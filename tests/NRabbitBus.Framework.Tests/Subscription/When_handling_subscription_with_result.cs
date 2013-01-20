using EvilDuck.Framework.Container;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests.Subscription
{
    public class When_handling_subscription_with_result
    {
        private MessageSubscriber _subscriber;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, this.GetType().Assembly));
            ComponentLocator.Current.Get<IModel>().QueueDeclare("TestQueue1_3", false, false, true, null);
        }

        [Test]
        public void Published_message_should_be_handled()
        {
            _subscriber = new MessageSubscriber(new MessageHandlerProvider(new MessageHandlerTypeCache(), null), ComponentLocator.Current.Get<IRabbitConnection>(), new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null);
            _subscriber.Subscribe(QueuesConfiguration.FromCode(new Queue()
                                                                   {
                                                                       Name = "TestQueue1_3",
                                                                       MaxThreads = 10
                                                                   }));
            
            new MessagePublisher(ComponentLocator.Current.Get<IModel>(),
                                 new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), new ResponseAwaiter(new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null), null)
                .Publish(new MyMessage2 { A = "A" }, "TestQueue1_3", message => TestContext.Handle.Set());

            TestContext.Handle.WaitOne();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Rabbit.Close();
        }
    }
}