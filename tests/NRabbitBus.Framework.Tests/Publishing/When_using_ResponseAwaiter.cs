using System;
using System.Threading;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.v0_9_1;

namespace NRabbitBus.Framework.Tests.Publishing
{
    [TestFixture]
    public class Having_ResponseAwaiter
    {
        [TestFixture]
        public class When_it_is_Initialized
        {
            private ResponseAwaiter _awaiter;
            private AutoResetEvent _threadLock = new AutoResetEvent(false);

            [TestFixtureSetUp]
            public void Setup()
            {
                Rabbit.Initialize();

                _awaiter =
                    new ResponseAwaiter(
                        new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null),
                        null);
                _awaiter.AwaiterInitialized += (sender, args) =>
                                                   {
                                                       _threadLock.Set();
                                                   };

                _awaiter.EnsureInitialized();
                _threadLock.WaitOne();
            }

            [Test]
            public void It_should_have_proper_QueueName()
            {
                _awaiter.QueueName.Should().NotBeEmpty();
            }

            [Test]
            public void It_should_have_a_listener_thread()
            {
                _awaiter.ResponseListenerThreadId.Should().NotBeEmpty();
            }

            [Test]
            public void IsListenerRunning_flag_should_be_true()
            {
                _awaiter.IsListenerRunning.Should().Be(true);
            }

            [TestFixtureTearDown]
            public void TearDown()
            {
                _awaiter.StopAwaiting();
                Rabbit.Close();
            }
        }

        [TestFixture]
        public class When_it_awaits_response
        {
            private ResponseAwaiter _awaiter;
            private AutoResetEvent _threadLock = new AutoResetEvent(false);

            [TestFixtureSetUp]
            public void Setup()
            {
                Rabbit.Initialize();

                _awaiter =
                    new ResponseAwaiter(
                        new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null),
                        null);

                _awaiter.EnsureInitialized();

            }

            [Test]
            public void Response_should_be_handled_properly()
            {
                IBasicProperties properties;
                _awaiter.RegisterResponseAction(m =>
                                                    {
                                                        _threadLock.Set();
                                                    }, out properties);

                var subscriptionId = properties.CorrelationId;
                var queueName = properties.ReplyTo;

                subscriptionId.Should().NotBeEmpty();
                queueName.Should().NotBeEmpty();

                var formatter = new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null),
                                                     null);


                var message = formatter.Format(new StringMessage()
                                                   {
                                                       Content = "abc"
                                                   });

                var channel = ComponentLocator.Current.Get<IModel>();
                channel.BasicPublish(String.Empty, queueName, new BasicProperties()
                                                                  {
                                                                      CorrelationId = subscriptionId
                                                                  }, message );

                _threadLock.WaitOne();

            }

            [TestFixtureTearDown]
            public void TearDown()
            {
                _awaiter.StopAwaiting();
                Rabbit.Close();
            }
        }
     
        [TestFixture]
       public class When_it_is_not_initialized
       {
            private ResponseAwaiter _awaiter;

            [TestFixtureSetUp]
            public void Setup()
            {
                Rabbit.Initialize();

                _awaiter =
                    new ResponseAwaiter(
                        new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null),
                        null);
                _awaiter.AwaiterInitialized += (sender, args) => Assert.Fail("It should never be initialized");
            }

            [Test]
            public void It_should_have_proper_QueueName()
            {
                _awaiter.QueueName.Should().BeEmpty();
            }

            [Test]
            public void It_should_have_a_listener_thread()
            {
                _awaiter.ResponseListenerThreadId.Should().BeEmpty();
            }

            [Test]
            public void IsListenerRunning_flag_should_be_true()
            {
                _awaiter.IsListenerRunning.Should().Be(false);
            }

            [TestFixtureTearDown]
            public void TearDown()
            {
                Rabbit.Close();
            }

       }

    }
}
