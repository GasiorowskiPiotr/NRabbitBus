using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace EvilDuck.Framework.Performance
{
    public abstract class CounterCategoryBase
    {
        protected abstract string CategoryName { get; }
        protected abstract IDictionary<string, PerformanceCounterType> DeclaredCounters { get; } 

        private readonly IDictionary<string, PerformanceCounter> _performanceCounters = new Dictionary<string, PerformanceCounter>();

        protected bool IsInitialized { get; private set; }

        internal void Initialize()
        {
            if(IsInitialized)
                return;
            
            if(!PerformanceCounterCategory.Exists(CategoryName))
            {
                var counterDataCollection = new CounterCreationDataCollection();

                foreach (var performanceCounter in DeclaredCounters)
                {
                    var counter = new CounterCreationData
                                      {
                                          CounterName = performanceCounter.Key, 
                                          CounterType = performanceCounter.Value
                                      };

                    counterDataCollection.Add(counter);
                }

                PerformanceCounterCategory.Create(CategoryName, String.Empty,
                                                  PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

                Thread.Sleep(2000);
            }

            foreach (var declaredCounter in DeclaredCounters)
            {
                var cnt = new PerformanceCounter(CategoryName, declaredCounter.Key, false);
                if(!_performanceCounters.ContainsKey(declaredCounter.Key))
                {
                    _performanceCounters.Add(declaredCounter.Key, cnt);
                }
            }

            IsInitialized = true;
        }

        protected bool TryGet(string performanceCounterName, out float result)
        {
            if(!IsInitialized)
                Initialize();

            result = 0.0f;
            if (!_performanceCounters.ContainsKey(performanceCounterName))
                return false;

            try
            {
                var counter = _performanceCounters[performanceCounterName];
                result = counter.NextValue();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool TrySet(string performanceCounterName, long value)
        {
            if (!IsInitialized)
                Initialize();

            if (!_performanceCounters.ContainsKey(performanceCounterName))
                return false;

            try
            {
                var counter = _performanceCounters[performanceCounterName];
                counter.RawValue = value;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool IncreaseBy(string performanceCounterName, int inc)
        {
            if (!IsInitialized)
                Initialize();

            if (!_performanceCounters.ContainsKey(performanceCounterName))
                return false;

            try
            {
                var counter = _performanceCounters[performanceCounterName];
                counter.IncrementBy(inc);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool DecreaseBy(string performanceCounterName)
        {
            if (!IsInitialized)
                Initialize();

            if (!_performanceCounters.ContainsKey(performanceCounterName))
                return false;

            try
            {
                var counter = _performanceCounters[performanceCounterName];
                counter.Decrement();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool Increase(string messagesProcessedPerSecond)
        {
            if (!IsInitialized)
                Initialize();

            if (!_performanceCounters.ContainsKey(messagesProcessedPerSecond))
                return false;

            try
            {
                var counter = _performanceCounters[messagesProcessedPerSecond];
                counter.Increment();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}