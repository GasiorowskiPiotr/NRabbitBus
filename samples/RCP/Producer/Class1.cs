using System;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("* In this scenario 999 messages are sent to 3 queues synchronuously (over 3 Bus instances).");

            Rabbit.Initialize();

            var bus1 = ComponentLocator.Current.Get<IBus>();
            var bus2 = ComponentLocator.Current.Get<IBus>();
            var bus3 = ComponentLocator.Current.Get<IBus>();

            for (int i = 0; i < 999; i++)
            {
                IBus bus;
                string queueName;
                if(i%3==0)
                {
                    bus = bus1;
                    queueName = "RcpQueue1";
                } 
                else if(i%3 == 1)
                {
                    bus = bus2;
                    queueName = "RcpQueue2";
                }
                else
                {
                    bus = bus3;
                    queueName = "RcpQueue3";
                }

                Console.WriteLine("{0} \t Sending message with sequenceNo {1} to Queue: {2}", DateTime.Now, i, queueName);

                var resp = bus.Request<RcpResponse>(new RcpRequest {SequenceNo = i}, queueName);

                Console.WriteLine("{0} \t Received message with SequenceNo {1}", DateTime.Now, queueName);

                if(resp.SequenceNo != i)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} \t Expected message with SequenceNo {1} instead received {2}", DateTime.Now, i, resp.SequenceNo);
                    Console.ResetColor();
                }

            }
        }
    }
}
