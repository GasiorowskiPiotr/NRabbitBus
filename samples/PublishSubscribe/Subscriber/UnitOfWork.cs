using System;
using NRabbitBus.Framework.Subscription;

namespace Subscriber
{
    public class UnitOfWork : IUnitOfWorkHandler
    {
        public void OnStartProcessing()
        {
            Console.WriteLine("UnitOfWork OnStartProcessing() called");
        }

        public void OnFinishedProcessing()
        {
            Console.WriteLine("UnitOfWork OnFinishedProcessing() called");
        }
    }
}