using System;
using Messages;
using NLog;
using NRabbitBus.Framework.Subscription;

namespace Subscriber2
{
    public class PublishedMessageSubscriber : MessageHandler<PublishedMessage>
    {
        public static int MessageCnt = 0;

        public PublishedMessageSubscriber(Logger logger)
            : base(logger)
        {
        }

        protected override void HandleMessage(PublishedMessage message)
        {
            if (MessageCnt > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This handler should process only one message in this scenario.");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} \t Received message ({1}) with text {2}", DateTime.Now, message.GetType(), message.Message);
            Console.ResetColor();

            Console.WriteLine("This Handler should be called SECOND.");

            if (message.Message != "This is a published test")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} \t Message expected was {1} but found {2}", DateTime.Now, "This is a published test", message.Message);
                Console.ResetColor();
            }
        }
    }
}