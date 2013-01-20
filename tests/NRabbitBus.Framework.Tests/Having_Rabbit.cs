using System;
using NRabbitBus.Framework.Configuration;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NUnit.Framework;

namespace NRabbitBus.Framework.Tests
{
    public class RabbitBusTestContext
    {
        public static bool RpcClient_Request_with_QueueName_Called;
        public static bool RpcClient_Request_with_Exchange_Called;
        public static bool Publisher_Publish_to_Queue_Called;
        public static bool Publisher_Publish_to_Exchange_Called;
        public static bool Publisher_Publish_to_Queue_with_Async_Response_Called;
    }

    public class DummyRpcClient : IRpcClient
    {
        public TResponse Request<TResponse>(IMessage message, string queueName) where TResponse : class, IMessage
        {
            RabbitBusTestContext.RpcClient_Request_with_QueueName_Called = true;

            return null;
        }

        public TResponse Request<TResponse>(IMessage message, string exchangeName, string exchangeType, string routingKey) where TResponse : class, IMessage
        {
            RabbitBusTestContext.RpcClient_Request_with_Exchange_Called = true;

            return null;
        }
    }

    public class DummyPublisher : IMessagePublisher
    {
        public void Publish(IMessage message, string queueName)
        {
            RabbitBusTestContext.Publisher_Publish_to_Queue_Called = true;
        }

        public void Publish(IMessage message, string exchangeName, string routingKey)
        {
            RabbitBusTestContext.Publisher_Publish_to_Exchange_Called = true;
        }

        public void Publish(IMessage message, string queueName, Action<IMessage> replyAction)
        {
            RabbitBusTestContext.Publisher_Publish_to_Queue_with_Async_Response_Called = true;
        }
    }


    [TestFixture]
    public class Having_Rabbit
    {
        private Action RabbitInitializer = () => Rabbit
                                                     .Initialize(typeof(Having_Rabbit).Assembly)
                                                     .DeclareExchanges(
                                                         ExchangesConfiguration.FromCode(new Exchange("a", "direct",
                                                                                                      false)))
                                                     .DeclareQueues(
                                                         QueuesConfiguration.FromCode(new Queue("b", false, false,
                                                                                                false, 10)))
                                                     .SetMessageHandlerOrder(MessageHandlersOrderConfiguration.Empty())
                                                     .SetupPerformanceCounters()
                                                     .SetupRouting(
                                                         RoutesConfiguration.FromCode(new Route("b", "a", "c")))
                                                     .SetupConnectionProperties(null)
                                                     .StartListeningOnDeclaredQueues();

        private Action RabbitCloser = () => Rabbit.Close();

        [Test]
        public void It_should_be_able_to_reinitialize()
        {
            RabbitInitializer();
            RabbitCloser();
            RabbitInitializer();
            RabbitCloser();
        }

        [Test]
        public void It_should_be_able_to_initialize_and_close()
        {
            RabbitInitializer();
            RabbitCloser();
        }

        [Test]
        public void Initializing_for_second_time_should_throw()
        {
            try
            {
                RabbitInitializer();
                RabbitInitializer();

                Assert.Fail();
            }
            catch (Exception)
            {
                Rabbit.Close();
            }
        }

        [Test]
        public void Closing_Rabbit_without_intialization_should_not_throw()
        {
            Rabbit.Close();
        }
    }
}
