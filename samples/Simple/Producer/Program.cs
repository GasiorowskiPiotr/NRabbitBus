using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit.Initialize();

            var bus = ComponentLocator.Current.Get<IBus>();

            Console.WriteLine("Publishing message for '3_1_Simple_Queue'");

            bus.Publish(new MessageForSimpleQueue
                            {
                                Message = "Hello1"
                            }, "3_1_Simple_Queue");


            Console.WriteLine("Publishing message for '3_1_RoutedExchange' with key 'MyKey1'");

            bus.Publish(new MessageForRoutedQueue
                            {
                                Message = "1"
                            }, "3_1_RoutedExchange", "MyKey1");

            Console.WriteLine("Publishing message for '3_1_RoutedExchange' with key 'MyKey2'");

            bus.Publish(new MessageForRoutedQueue
                            {
                                Message = "2"
                            }, "3_1_RoutedExchange", "MyKey2");

            Console.WriteLine("Finished scenario. Verify whether all (3) messages were properly handled");
            Console.ReadLine();
        }
    }
}
