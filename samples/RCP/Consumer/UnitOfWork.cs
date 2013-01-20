using System;
using NRabbitBus.Framework.Subscription;

namespace Consumer
{
    public class UnitOfWork : IUnitOfWorkHandler
    {
        public void OnStartProcessing()
        {
            Console.WriteLine("Calling OnStartProcessing()");
        }

        public void OnFinishedProcessing()
        {
            Console.WriteLine("Calling OnFinishedProcessing()");
        }
    }
}