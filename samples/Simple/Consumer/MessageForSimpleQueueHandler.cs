using System;
using Messages;
using NLog;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    public class MessageForSimpleQueueHandler : MessageHandler<MessageForSimpleQueue>
    {
        public static int MessageCnt = 0;

        public MessageForSimpleQueueHandler(Logger logger) : base(logger)
        {
        }

        protected override void HandleMessage(MessageForSimpleQueue message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{2} \t Received message ({0}) with text: {1}", message.GetType(), message.Message, DateTime.Now);
            Console.ResetColor();

            MessageCnt++;

            if (MessageCnt > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} \t This handler should receive only one message in thie scenarion", DateTime.Now);
                Console.ResetColor();
            }

            if(message.Message != "Hello1")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} \t Received wrong message. The message should be 'Hello1' but it was {1}", DateTime.Now, message.Message);
                Console.ResetColor();
            }
        }
    }
}