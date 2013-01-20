using System;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NLog;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests
{
    [TestFixture]
    public class Having_container_bootstrapped
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, GetType().Assembly));
        }

        [Test]
        public void It_should_have_ConnectionFactory()
        {
            ComponentLocator.Current.Get<ConnectionFactory>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_always_one_instance_of_ConnectionFactory()
        {
            var a = ComponentLocator.Current.Get<ConnectionFactory>();
            var b = ComponentLocator.Current.Get<ConnectionFactory>();

            a.Should().BeSameAs(b);
        }

        [Test]
        public void It_should_have_Connection_as_IConnection()
        {
            ComponentLocator.Current.Get<IRabbitConnection>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_one_instance_of_Connection()
        {
            var a = ComponentLocator.Current.Get<IRabbitConnection>();
            var b = ComponentLocator.Current.Get<IRabbitConnection>();

            a.Should().BeSameAs(b);
        }

        [Test]
        public void It_should_have_Model_as_IModel()
        {
            ComponentLocator.Current.Get<IModel>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_transient_Model()
        {
            var a = ComponentLocator.Current.Get<IModel>();
            var b = ComponentLocator.Current.Get<IModel>();

            a.Should().NotBeSameAs(b);
        }

        [Test]
        public void It_should_have_MessageFormatter()
        {
            ComponentLocator.Current.Get<IMessageFormatter>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_custom_MessageFormatters()
        {
            ComponentLocator.Current.Get<IExtendedMessageFormatter>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_MessageStringifier()
        {
            ComponentLocator.Current.Get<IMessageStringifier>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_custom_MessageStringifier()
        {
            ComponentLocator.Current.Get<IExtendedMessageStringifier>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_MessageSerializer()
        {
            ComponentLocator.Current.Get<IMessageSerializer>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_custom_MessageSerializer()
        {
            ComponentLocator.Current.Get<IExtendedMessageSerializer>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_MessagePublisher()
        {
            ComponentLocator.Current.Get<IMessagePublisher>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_MessageSubscriber()
        {
            ComponentLocator.Current.Get<IMessageSubscriber>().Should().NotBeNull();
        }

        [Test]
        public void It_should_have_MessageHandlers_from_Assembly()
        {
            ComponentLocator.Current.Get<MessageHandler>().Should().NotBeNull();
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            ComponentLocator.Dispose();
        }
    }

    public class TestMessageHandler : MessageHandler<TestMessage>
    {
        public TestMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(TestMessage message)
        {
            
        }
    }

    public class TestMessage : IMessage
    {
        
    }

    public class ExtendedMessageSerializer : IExtendedMessageSerializer
    {
        public byte[] Serialize(string objStringForm)
        {
            throw new NotImplementedException();
        }

        public string Deserialize(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialize(ref string objStringForm)
        {
            throw new NotImplementedException();
        }

        public void OnAfterDeserialize(ref string objStringForm)
        {
            throw new NotImplementedException();
        }
    }

    public class ExtendedMessageStringifier : IExtendedMessageStringifier
    {
        public string Stringify(object obj)
        {
            throw new NotImplementedException();
        }

        public object Destringify(string objStringForm)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeStringify(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnAfterStringify(ref string objStringForm)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeDestringify(ref string objStringForm)
        {
            throw new NotImplementedException();
        }

        public void OnAfterDestringify(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class SomeExtendedMessageFormatter : IExtendedMessageFormatter
    {
        public byte[] Format(object obj)
        {
            throw new NotImplementedException();
        }

        public object Deformat(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeFormat(object obj)
        {
            throw new NotImplementedException();
        }

        public void OnAfterDeformat(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
