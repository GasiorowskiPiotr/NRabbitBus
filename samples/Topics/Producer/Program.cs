using System;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit
                .Initialize();

            Console.WriteLine("Publishing message with routing key that should be handled only by one handler");

            var bus = ComponentLocator.Current.Get<IBus>();
            bus.Publish(new TopicMessage
            {
                Text = "piotr.#"
            }, "topic1_exchange", "piotr.rzeznik.economist");

            Console.ReadLine();
            Console.WriteLine("Publishing message with routing key that should be handled by two handlers");

            bus.Publish(new TopicMessage
            {
                Text = "piotr.#"
            }, "topic1_exchange", "piotr.gasiorowski.economist");

            Console.ReadLine();
            Console.WriteLine("Publishing message with routing key that should be handled by three handlers");

            bus.Publish(new TopicMessage
            {
                Text = "piotr.#"
            }, "topic1_exchange", "piotr.gasiorowski.programmer");

            Console.ReadLine();
        }
    }
}
