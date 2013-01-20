using System;
using FluentAssertions;
using NLog;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Subscription
{
    [TestFixture]
    public class Having_MessageHandler
    {

        [TestFixture]
        public class When_exception_is_thrown
        {
            private readonly MessageHandler _handler = new TestMessageHandler(null);

            [SetUp]
            public void Setup()
            {
                _handler.Handle(new StringMessage());
            }

            [Test]
            public void StopProcessing_flag_should_be_true()
            {
                _handler.StopProcessing.Should().BeTrue();
            }

            [Test]
            public void StopProcessingReason_should_point_to_exception()
            {
                _handler.StopProcessingReason.Should().NotBeNull();
                _handler.StopProcessingReason.Should().BeAssignableTo<Exception>();
            }
        }

        [TestFixture]
        public class When_message_of_other_type_is_passed
        {
            private readonly MessageHandler _handler = new TestMessageHandler(null);

            [Test]
            public void Should_not_fail()
            {
                Assert.DoesNotThrow(() => _handler.Handle(new OtherMessage()));
            }
        }

        [TestFixture]
        public class When_message_of_specific_type_is_passed
        {
            private readonly MessageHandler _handler = new TestMessageHandler(null);

            [Test]
            public void Should_not_fail()
            {
                Assert.DoesNotThrow(() => _handler.Handle(new StringMessage()));
            }
        }

        [TestFixture]
        public class When_message_of_derived_type_is_passed
        {
            private readonly MessageHandler _handler = new TestMessageHandler(null);

            [Test]
            public void Should_not_fail()
            {
                Assert.DoesNotThrow(() => _handler.Handle(new StringDerivedMessage()));
            }
        }


        private class TestMessageHandler : MessageHandler<StringMessage>
        {
            public TestMessageHandler(Logger logger) : base(logger)
            {
                StopOnFailure = true;
            }

            protected override void HandleMessage(StringMessage message)
            {
                throw new Exception("TestException");
            }
        }

        private class OtherMessage : IMessage
        {
            
        }

        public class StringDerivedMessage : StringMessage
        {
            
        }

    }
}
