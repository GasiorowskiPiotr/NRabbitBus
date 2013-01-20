using System;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit.Initialize();

            var bus = ComponentLocator.Current.Get<IBus>();

            Console.WriteLine("Publishing a message. All the Subscribers should get it.");

            bus.Publish(new PublishedMessage
                            {
                                Message = "This is a published test"
                            }, "PubSubExchange", String.Empty);

            Console.WriteLine("Message published.");
            Console.ReadLine();
        }
    }
}
