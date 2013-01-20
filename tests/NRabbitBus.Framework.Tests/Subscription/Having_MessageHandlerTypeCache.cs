using System;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NLog;
using NRabbitBus.Framework.Shared;
using NRabbitBus.Framework.Subscription;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests.Subscription
{
    public class Having_MessageHandlerTypeCache
    {
        [TestFixture]
        public class Getting_MessageHandlersTypes_for_Message
        {
            [SetUp]
            public void Setup()
            {
                Rabbit.Initialize(this.GetType().Assembly);
            }

            [Test]
            public void Should_expire_after_a_year()
            {
                var cache = ComponentLocator.Current.Get<IMessageHandlerTypeCache>();
                cache.ItemLifeSpan.Should().Be(TimeSpan.FromDays(365));
            }

            [Test]
            public void Should_return_two_handler_types_if_present()
            {
                var cache = ComponentLocator.Current.Get<IMessageHandlerTypeCache>();
                var handlerTypes = cache.Get(typeof (MessageHandlerTypeCacheMessage));

                handlerTypes.Should().NotBeNull();
                handlerTypes.Should().NotBeEmpty();
                handlerTypes.Should().Contain(typeof (MessageHandler<MessageHandlerTypeCacheMessage>));
                handlerTypes.Should().Contain(typeof (MessageHandlerWithResult<MessageHandlerTypeCacheMessage>));
            }

            [Test]
            public void Should_return_handler_types_for_base_messages_if_present()
            {
                var cache = ComponentLocator.Current.Get<IMessageHandlerTypeCache>();
                var handlerTypes = cache.Get(typeof(MessageHandlerTypeCacheDerivedMessage));

                handlerTypes.Should().NotBeNull();
                handlerTypes.Should().NotBeEmpty();
                handlerTypes.Should().Contain(typeof(MessageHandler<MessageHandlerTypeCacheMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandlerWithResult<MessageHandlerTypeCacheMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandler<MessageHandlerTypeCacheDerivedMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandlerWithResult<MessageHandlerTypeCacheDerivedMessage>));
            }

            [Test]
            public void Should_return_handler_types_for_entire_hierarchy_of_messages_if_present()
            {
                var cache = ComponentLocator.Current.Get<IMessageHandlerTypeCache>();
                var handlerTypes = cache.Get(typeof(MessageHandlerTypeCacheDerivedMessage2));

                handlerTypes.Should().NotBeNull();
                handlerTypes.Should().NotBeEmpty();
                handlerTypes.Should().Contain(typeof(MessageHandler<MessageHandlerTypeCacheMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandlerWithResult<MessageHandlerTypeCacheMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandler<MessageHandlerTypeCacheDerivedMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandlerWithResult<MessageHandlerTypeCacheDerivedMessage>));
                handlerTypes.Should().Contain(typeof(MessageHandler<MessageHandlerTypeCacheDerivedMessage2>));
                handlerTypes.Should().Contain(typeof(MessageHandlerWithResult<MessageHandlerTypeCacheDerivedMessage2>));
            }

            [TearDown]
            public void TearDown()
            {
                Rabbit.Close();
            }
        }

    }

    public class MessageHandlerTypeCacheMessage : IMessage
    {
        
    }

    public class Handler1 : MessageHandler<MessageHandlerTypeCacheMessage>
    {
        public Handler1(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(MessageHandlerTypeCacheMessage message)
        {
            
        }
    }

    public class Handler1WithResult : MessageHandlerWithResult<MessageHandlerTypeCacheMessage>
    {
        public Handler1WithResult(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(MessageHandlerTypeCacheMessage message)
        {
            return   new StringMessage();
        }
    }

    public class MessageHandlerTypeCacheDerivedMessage : MessageHandlerTypeCacheMessage
    {
        
    }

    public class MessageHandlerTypeCacheDerivedMessage2 : MessageHandlerTypeCacheDerivedMessage
    {

    }

    public class Handler2 : MessageHandler<MessageHandlerTypeCacheDerivedMessage>
    {
        public Handler2(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(MessageHandlerTypeCacheDerivedMessage message)
        {
            
        }
    }

    public class Handler2WithResult : MessageHandlerWithResult<MessageHandlerTypeCacheDerivedMessage>
    {
        public Handler2WithResult(Logger logger) : base(logger)
        {
        }

        protected override IMessage HandleMessage(MessageHandlerTypeCacheDerivedMessage message)
        {
            return new StringMessage();
        }
    }

    public class Handler3 : MessageHandler<MessageHandlerTypeCacheDerivedMessage2>
    {
        public Handler3(Logger logger)
            : base(logger)
        {
        }

        protected override void HandleMessage(MessageHandlerTypeCacheDerivedMessage2 message)
        {

        }
    }

    public class Handler3WithResult : MessageHandlerWithResult<MessageHandlerTypeCacheDerivedMessage2>
    {
        public Handler3WithResult(Logger logger)
            : base(logger)
        {
        }

        protected override IMessage HandleMessage(MessageHandlerTypeCacheDerivedMessage2 message)
        {
            return new StringMessage();
        }
    }
}
