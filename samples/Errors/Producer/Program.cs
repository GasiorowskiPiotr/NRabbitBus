using System;
using System.Diagnostics;
using System.Threading;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;
using NRabbitBus.Framework.Shared;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rabbit.Initialize();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    var bus = ComponentLocator.Current.Get<IBus>();
                    bus.Publish(new TestMessageWithPublishDate
                    {
                        PublishDate = DateTime.Now,
                        Text = "Message"
                    }, "SometimesDisconnectedQueue");

                    Thread.Sleep(TimeSpan.FromSeconds(0.01));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            sw.Stop();

            Console.WriteLine("Published 10000 messages in {0} [ms]", sw.ElapsedMilliseconds);
            Console.ReadLine();
            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    var bus = ComponentLocator.Current.Get<IBus>();
                    var resp = bus.Request<StringMessage>(new TestRpcMessageWithPublishDate
                                                              {
                                                                  PublishDate = DateTime.Now,
                                                                  Text = "Message",
                                                                  SequenceNo = i
                                                              }, "SometimesDisconnectedRpcQueue");

                    Console.WriteLine("{0} \t Received message with content {1}", DateTime.Now, resp.Content);
                    Thread.Sleep(TimeSpan.FromSeconds(0.01));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            sw.Stop();

            Console.WriteLine("Handled 10000 RPC calls in {0} [ms]", sw.ElapsedMilliseconds);

            Console.ReadLine();

            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    var bus = ComponentLocator.Current.Get<IBus>();
                    bus.Publish(new TestRpcMessageWithPublishDate
                    {
                        PublishDate = DateTime.Now,
                        Text = "Message",
                        SequenceNo = i
                    }, "SometimesDisconnectedAsyncQueue", 
                    m =>
                        {
                            var stringMessage = m as StringMessage;
                            Console.WriteLine(stringMessage != null
                                                  ? stringMessage.Content
                                                  : "Returned message is not a StringMessage");
                        });

                    Thread.Sleep(TimeSpan.FromSeconds(0.01));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            sw.Stop();

            Console.WriteLine("Published 10000 messages with async response in {0} [ms]", sw.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
