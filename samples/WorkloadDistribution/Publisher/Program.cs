using System;
using System.Threading;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("* This publisher created infinite number of messages sent per 100 ms.");

            Rabbit.Initialize();
            
            var r = new Random();
            var bus = ComponentLocator.Current.Get<IBus>();
            
            while(true)
            {
                Console.WriteLine("Sending a message...");

                bus.Publish(new WaitMsMessage
                                {
                                    MilisecondsToWait = r.Next() % 10000
                                }, "WorkQueue");

                Console.WriteLine("Message sent.");
                Thread.Sleep(100);
            }
        }
    }
}
