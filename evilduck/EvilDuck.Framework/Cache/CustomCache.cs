using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EvilDuck.Framework.Cache
{
    public abstract class CustomCache<TCache, TCacheKey, TCachedData> where TCache : CustomCache<TCache, TCacheKey, TCachedData>
    {
        private static readonly ConcurrentDictionary<TCacheKey, CustomCacheItem> CachedItems = new ConcurrentDictionary<TCacheKey, CustomCacheItem>();

        protected abstract TCachedData OnMiss(TCacheKey key);
        protected abstract TimeSpan ItemLifeTime { get; }

        public TCachedData Get(TCacheKey key)
        {
            if (CachedItems.ContainsKey(key))
            {
                var item = CachedItems[key];
                if (item.CreatedOn + ItemLifeTime < DateTime.Now)
                {
                    var newData = OnMiss(key);
                    CustomCacheItem oldValue;
                    CachedItems.TryRemove(key, out oldValue);
                    CachedItems.TryAdd(key, new CustomCacheItem(newData, key));
                }
            }
            else
            {
                var newData = OnMiss(key);
                CachedItems.TryAdd(key, new CustomCacheItem(newData, key));
            }
            
            return CachedItems[key].Data;
        }

        public void Add(TCacheKey key, TCachedData data)
        {
            CachedItems.TryAdd(key, new CustomCacheItem(data, key));
        }

        public bool Contains(TCacheKey key)
        {
            return CachedItems.ContainsKey(key);
        }

        public void Invalidate()
        {
            CachedItems.Clear();
        }

        public void RemoveOutdated()
        {
            var toRemove = CachedItems.Where(item => DateTime.Now - item.Value.CreatedOn > ItemLifeTime).Select(item => item.Key).ToList();
            foreach (var cacheKey in toRemove)
            {
                CustomCacheItem item;
                CachedItems.TryRemove(cacheKey, out item);
            }
        }

        internal class CustomCacheItem
        {
            public CustomCacheItem(TCachedData data, TCacheKey key)
            {
                Data = data;
                Key = key;
                CreatedOn = DateTime.Now;
            }

            public TCachedData Data { get; private set; }
            public TCacheKey Key { get; private set; }
            public DateTime CreatedOn { get; private set; }
        }
    }
}
