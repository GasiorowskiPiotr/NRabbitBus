using System;
using Autofac;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NLog;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Subscription
{
    [TestFixture]
    public class Having_class_derived_from_MessagePipelineBase
    {
        [TestFixture]
        public class Calling_Process_on_message
        {
            private IMessage _message;

            [TestFixtureSetUp]
            public void SetUp()
            {
                Rabbit.Initialize(GetType().Assembly);

                var pipeline = new TestMessagePipeline(ComponentLocator.Current.Get<IMessageHandlerProvider>(),
                                        ComponentLocator.Current.StartChildScope(), new TestMessage(), null);

                pipeline.Process(out _message);
            }

            [Test]
            public void Should_return_non_Null_message()
            {
                _message.Should().NotBeNull();
            }

            [Test]
            public void Should_handle_UnitOfWork_if_exists()
            {
                PipelineTestContext.IsOnAfterProcessingCalled.Should().BeTrue();
                PipelineTestContext.IsOnBeforeProcessingCalled.Should().BeTrue();
            }

            [Test]
            public void Should_handle_message_in_MessageHandler_properly()
            {
                PipelineTestContext.IsStopProcessingCalled.Should().BeTrue();
                PipelineTestContext.IsStopProcessingReasonSet.Should().BeTrue();
                PipelineTestContext.IsHandleMessageCalled.Should().BeTrue();
            }

            [TestFixtureTearDown]
            public void TearDown()
            {
                Rabbit.Close();
                
            }
        }
    }

    public class TestMessagePipeline : MessagePipelineBase
    {
        public TestMessagePipeline(IMessageHandlerProvider messageHandlerProvider, ILifetimeScope lifetimeScope, IMessage message, Logger logger) : base(messageHandlerProvider, lifetimeScope, message, logger)
        {
        }
    }

    public class TestMessage : IMessage
    {
        
    }

    public class TestMessageHandler : MessageHandlerWithResult<TestMessage>
    {
        public TestMessageHandler(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(TestMessage message)
        {
            PipelineTestContext.IsHandleMessageCalled = true;

            this.StopProcessing = true;

            PipelineTestContext.IsStopProcessingCalled = true;

            this.StopProcessingReason = new Exception("Something wrong");

            PipelineTestContext.IsStopProcessingReasonSet = true;

            return new StringMessage { Content = "EverythingOk" };
        }
    }

    public class UnitOfWork : IUnitOfWorkHandler
    {
        public void OnStartProcessing()
        {
            PipelineTestContext.IsOnBeforeProcessingCalled = true;
        }

        public void OnFinishedProcessing()
        {
            PipelineTestContext.IsOnAfterProcessingCalled = true;
        }
    }

    public static class PipelineTestContext
    {
        public static bool IsOnBeforeProcessingCalled;
        public static bool IsOnAfterProcessingCalled;
        public static bool IsHandleMessageCalled;
        public static bool IsStopProcessingCalled;
        public static bool IsStopProcessingReasonSet;
    }
}
