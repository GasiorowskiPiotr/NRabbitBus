using System.Collections.Generic;
using System.Diagnostics;
using EvilDuck.Framework.Performance;

namespace NRabbitBus.Framework.Performance
{
    public class RabbitPerformance : CounterCategoryBase
    {
        protected override string CategoryName
        {
            get { return "NRabbitBus"; }
        }

        protected override IDictionary<string, PerformanceCounterType> DeclaredCounters
        {
            get
            {
                return new Dictionary<string, PerformanceCounterType>
                           {
                               {"Messages Processed per Second", PerformanceCounterType.RateOfCountsPerSecond64},
                               {"Threads Currently used", PerformanceCounterType.NumberOfItems64},
                               {"Threads Used per Second", PerformanceCounterType.RateOfCountsPerSecond64}
                           };
            }
        }

        public void IncrementThreadsCurrentlyUsed()
        {
            Increase("Threads Currently used");
        }

        public void IncreaseMessagesProcessedPerSecond()
        {
            Increase("Messages Processed per Second");
        }

        public void DecrementThreadsCurrentlyUsed()
        {
            DecreaseBy("Threads Currently used");
        }

        public void IncreaseThreadsUsedPerSecond()
        {
            Increase("Threads Used per Second");
        }
    }
}
