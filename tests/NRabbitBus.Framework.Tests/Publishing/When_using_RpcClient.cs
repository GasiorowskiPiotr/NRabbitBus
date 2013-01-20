using System.Threading;
using EvilDuck.Framework.Container;
using FluentAssertions;
using NRabbitBus.Framework.MessageProcess;
using NRabbitBus.Framework.Publishing;
using NRabbitBus.Framework.Shared;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace NRabbitBus.Framework.Tests.Publishing
{
    [TestFixture]
    public class When_using_RpcClient
    {
        private IRpcClient _rpcClient;
        private Thread _serverThread;

        private bool _isInitialized = false;

        [TestFixtureSetUp]
        public void Setup()
        {
            Rabbit.Initialize();

            _rpcClient =
                new RpcClient(
                    new MessageFormatter(new Utf8MessageSerializer(null), new JsonMessageStringifier(null), null), 
                    ComponentLocator.Current.Get<IModel>(),
                    null);

            var model = ComponentLocator.Current.Get<IModel>();
            model.QueueDeclare("Rpc_Test_Queue_2_2_2", false, false, true, null);
            model.ExchangeDeclare("Rcp_Test_Exchange_2_2_2", "direct", false);
            model.QueueBind("Rpc_Test_Queue_2_2_2", "Rcp_Test_Exchange_2_2_2", "abc");

            _serverThread = new Thread(StartRpcServer);
            _serverThread.Start();
        }

        [Test]
        public void It_should_be_able_to_send_and_receive_messages_sent_directly_to_queue()
        {
            Thread.Sleep(1000);

            var resp = _rpcClient.Request<StringMessage>(new StringMessage
                                                             {
                                                                 Content = "QueueTest"
                                                             }, "Rpc_Test_Queue_2_2_2");

            resp.Content.Should().Be("QueueTest");
        }

        [Test]
        public void It_should_be_able_to_send_and_receive_messages_sent_to_exchange()
        {
            Thread.Sleep(1000);

            var resp = _rpcClient.Request<StringMessage>(new StringMessage
            {
                Content = "ExchangeTest"
            }, "Rcp_Test_Exchange_2_2_2", "direct", "abc");

            resp.Content.Should().Be("ExchangeTest");
        }

        private void StartRpcServer()
        {
            new LoopRpcServer(
                new RabbitMQ.Client.MessagePatterns.Subscription(
                    ComponentLocator.Current.Get<IModel>(),
                    "Rpc_Test_Queue_2_2_2"))
                .MainLoop();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _serverThread.Join(500);
            Rabbit.Close();
        }
    }

    public class LoopRpcServer : SimpleRpcServer
    {
        public LoopRpcServer(RabbitMQ.Client.MessagePatterns.Subscription subscription) : base(subscription)
        {
        }

        public override byte[] HandleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            replyProperties = null;
            return body;
        }
    }
}
