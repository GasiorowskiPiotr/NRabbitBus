using System;

namespace EvilDuck.Framework.Performance
{
    public class Performance<TCounter> where TCounter : CounterCategoryBase
    {
        private static readonly Lazy<TCounter> LazyCounter = new Lazy<TCounter>(); 

        public static TCounter Counter
        {
            get { return LazyCounter.Value; }
        }

        public static void Init()
        {
            LazyCounter.Value.Initialize();
        }
    }
}
