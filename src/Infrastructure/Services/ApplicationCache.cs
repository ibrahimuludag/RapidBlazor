using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using RapidBlazor.Application.Common.Interfaces;
using System;

namespace RapidBlazor.Infrastructure.Services
{
    public class ApplicationCache : IApplicationCache
    {
        private IMemoryCache _cache;

        public ApplicationCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public object Get(object key)
        {
            return _cache.Get(key);
        }

        public TItem Get<TItem>(object key)
        {
            return _cache.Get<TItem>(key);
        }

        public TItem Set<TItem>(object key, TItem value)
        {
            return _cache.Set(key, value);
        }

        public TItem Set<TItem>(object key, TItem value, IChangeToken expirationToken)
        {
            return _cache.Set(key, value, expirationToken);
        }

        public TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            return _cache.Set(key, value, absoluteExpiration);
        }

        public TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            return _cache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
