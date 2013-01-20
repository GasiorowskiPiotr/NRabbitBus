using System.Collections.Generic;
using System.Threading;
using EvilDuck.Framework.Container;
using NLog;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests.Subscription
{
    [TestFixture]
    public class When_handling_subscription
    {
        private MessageSubscriber _subscriber;

        [TestFixtureSetUp]
        public void SetUp()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, this.GetType().Assembly));
            ComponentLocator.Current.Get<IModel>().QueueDeclare("TestQueue1_2", false, false, true, null);
        }

        [Test]
        public void Published_message_should_be_handled()
        {


            _subscriber = new MessageSubscriber(new MessageHandlerProvider(new MessageHandlerTypeCache(), null), ComponentLocator.Current.Get<IRabbitConnection>(), new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null);
            _subscriber.Subscribe(QueuesConfiguration.FromCode(new Queue()
                                                                   {
                                                                       Name = "TestQueue1_2",
                                                                       MaxThreads = 10
                                                                   }));
                                     
            new MessagePublisher(ComponentLocator.Current.Get<IModel>(),
                                 new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), new ResponseAwaiter(new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null ), null)
                                 .Publish(new MyMessage { Msg = "A" }, "TestQueue1_2");

            TestContext.Handle.WaitOne();

            
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Rabbit.Close();
        }
    }

    public class MyMessage : IMessage
    {
        public string Msg { get; set; }
    }

    public class TestContext
    {
        public static AutoResetEvent Handle = new AutoResetEvent(false);
    }

    public class MyMessageHandler : MessageHandler<MyMessage>
    {
        public MyMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(MyMessage message)
        {
            TestContext.Handle.Set();
        }
    }

    public class MyMessage3 : IMessage
    {
        public string A { get; set; }
    }

    public class MyResultMessage3 : IMessage
    {
        public string B { get; set; }
    }

    public class MyMessageHandler3 : MessageHandlerWithResult<MyMessage3>
    {
        public MyMessageHandler3(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(MyMessage3 message)
        {
            return new MyResultMessage3
                       {
                           B = message.A
                       };
        }
    }

    public class MyMessage2 : IMessage
    {
        public string A { get; set; }
    }

    public class MyMessageHandler2 : MessageHandlerWithResult<MyMessage2>
    {
        public MyMessageHandler2(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(MyMessage2 message)
        {
            return new StringMessage
                       {
                           Content = "AbcAbcAbc"
                       };
        }
    }
}
