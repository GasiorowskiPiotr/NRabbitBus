using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NRabbitBus.Framework.Tests.Publishing
{
    [TestFixture]
    public class While_using_MessagePublisher
    {
        [SetUp]
        public void Setup()
        {
            ContainerBootstrap.Initialize(new NRabbitModule(true, this.GetType().Assembly));

            var model = ComponentLocator.Current.Get<IModel>();
            model.QueueDeclare("TestQueue1_1", false, false, true, null);
            model.ExchangeDeclare("TestExchange1_1", "direct", false);
            model.QueueBind("TestQueue1_1", "TestExchange1_1", "abc");
        }

        [Test]
        public void It_should_sent_messages_which_will_be_received()
        {

            var model1 = ComponentLocator.Current.Get<IModel>();
            var publisher = new MessagePublisher(model1,
                                                 new MessageFormatter(new Utf8MessageSerializer(null),
                                                                      new JsonMessageStringifier(null), null), new ResponseAwaiter(new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null ), null ),  null);
            publisher.Publish(new SampleMessage(){ Message = "Hello"}, "TestQueue1_1" );

            var model = ComponentLocator.Current.Get<IModel>();
            var subs = new RabbitMQ.Client.MessagePatterns.Subscription(model, "TestQueue1_1");
            var args = subs.Next();

            var messageFormatter = new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null);
            var obj = messageFormatter.Deformat(args.Body);

            obj.Should().NotBeNull();
            obj.Should().BeOfType<SampleMessage>();

            ((SampleMessage) obj).Message.Should().Be("Hello");

            subs.Close();
        }

        [Test]
        public void It_should_publish_message_to_exchange_and_it_will_be_received()
        {
            var model1 = ComponentLocator.Current.Get<IModel>();
            var publisher = new MessagePublisher(model1,
                                                 new MessageFormatter(new Utf8MessageSerializer(null),
                                                                      new JsonMessageStringifier(null), null), new ResponseAwaiter(new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), null), null);
            publisher.Publish(new SampleMessage() { Message = "Hello" }, "TestExchange1_1", "abc");

            var model = ComponentLocator.Current.Get<IModel>();
            var subs = new RabbitMQ.Client.MessagePatterns.Subscription(model, "TestQueue1_1");
            var args = subs.Next();

            var messageFormatter = new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null);
            var obj = messageFormatter.Deformat(args.Body);

            obj.Should().NotBeNull();
            obj.Should().BeOfType<SampleMessage>();

            ((SampleMessage)obj).Message.Should().Be("Hello");

            subs.Close();
        }

        [TearDown]
        public void TearDown()
        {
            ComponentLocator.Dispose();
        }
    }

    public class SampleMessage : IMessage
    {
        public string Message { get; set; }
    }
}
