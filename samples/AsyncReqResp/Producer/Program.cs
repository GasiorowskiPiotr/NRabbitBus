using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EvilDuck.Framework.Container;
using Messages;
using NRabbitBus.Framework;

namespace Producer
{
    class Program
    {
        static readonly IList<Caller> Callers = new List<Caller>(1998); 

        static void Main(string[] args)
        {
            Console.WriteLine("* This scenario produces Asynchronuous Req/Res messages to 3 queues and awaits their responses");

            Rabbit.Initialize();

            Console.WriteLine("Starting publishing to 3 queues on a single thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var bus = ComponentLocator.Current.Get<IBus>();

            for (int i = 0; i < 999; i++)
            {
                string queueName = String.Empty;
                if (i%3 == 0)
                    queueName = "AsyncReqResp1";
                if (i % 3 == 1)
                    queueName = "AsyncReqResp2";
                if (i % 3 == 2)
                    queueName = "AsyncReqResp3";

                var caller = new Caller(i, bus, queueName);
                caller.Do();
                Callers.Add(caller);
            }

            Console.WriteLine("Starting publishing to 3 queues on a multiple threads");

            Parallel.For(0, 999, i =>
                                     {
                                         string queueName = String.Empty;
                                         if (i % 3 == 0)
                                             queueName = "AsyncReqResp1";
                                         if (i % 3 == 1)
                                             queueName = "AsyncReqResp2";
                                         if (i % 3 == 2)
                                             queueName = "AsyncReqResp3";

                                         var caller = new Caller(i, bus, queueName);
                                         caller.Do();
                                         Callers.Add(caller);
                                     });

            Console.ReadLine();
        }
    }

    class Caller
    {
        private readonly int _seqNo;
        private readonly IBus _bus;
        private readonly string _queueName;

        public Caller(int seqNo, IBus bus, string queueName)
        {
            _seqNo = seqNo;
            _bus = bus;
            _queueName = queueName;
        }

        public void Do()
        {
            _bus.Publish(new RequestMessage
                                {
                                    SequenceNo = _seqNo
                                }, _queueName, message =>
                                                   {
                                                       var response = message as ResponseMessage;
                                                       if(response == null)
                                                       {
                                                           Console.ForegroundColor = ConsoleColor.Red;
                                                           Console.WriteLine("{0} \t Response message is not of type {1}", DateTime.Now, typeof(ResponseMessage));
                                                           Console.ResetColor();
                                                           return;
                                                       }

                                                       if(response.SequenceNo != _seqNo)
                                                       {
                                                           Console.ForegroundColor = ConsoleColor.Red;
                                                           Console.WriteLine("{0} \t Response message has SeqNo: {1} but request had {2}", DateTime.Now, response.SequenceNo, _seqNo);
                                                           Console.ResetColor();
                                                           return;
                                                       }

                                                       Console.WriteLine("{0} OK Req {1} - Resp {2} on Thread {3}", DateTime.Now, _seqNo, response.SequenceNo, Thread.CurrentThread.ManagedThreadId);
                                                   });
        }
    }
}
