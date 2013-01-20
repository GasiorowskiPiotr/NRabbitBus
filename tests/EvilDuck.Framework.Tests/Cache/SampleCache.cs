using System;
using System.Collections.Generic;
using EvilDuck.Framework.Cache;

namespace EvilDuck.Framework.Tests.Cache
{
    public class SampleCache : CustomCache<SampleCache, int, string>
    {
        public Dictionary<int, int> OnMissCounter = new Dictionary<int, int>();

        protected override string OnMiss(int key)
        {
            if (OnMissCounter.ContainsKey(key))
                OnMissCounter[key]++;
            else
                OnMissCounter.Add(key, 1);

            return "OnMiss:" + key;
        }

        protected override TimeSpan ItemLifeTime
        {
            get { return TimeSpan.FromSeconds(5); }
        }
    }
}