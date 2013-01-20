using System;
using Messages;
using NLog;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    public class MessageForRoutedQueueHandler : MessageHandler<MessageForRoutedQueue>
    {
        public static int MessageCnt = 0;

        public MessageForRoutedQueueHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(MessageForRoutedQueue message)
        {
            MessageCnt++;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{3} \t Received {0}/2 message ({1}) with text: {2} ", MessageCnt, message.GetType(), message.Message, DateTime.Now);
            Console.ResetColor();

            if(MessageCnt == 1)
                Console.WriteLine("Please note that if second message does not come - it should be considered an error!");

            if (MessageCnt > 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} \t This handler should receive only two messages in thie scenarion", DateTime.Now);
                Console.ResetColor();
            }

        }
    }
}