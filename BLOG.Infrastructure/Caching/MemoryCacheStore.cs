using BLOG.Application.Caching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Caching
{
    public class MemoryCacheStore : ICacheStore
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
            
        public void Add<TItem>(TItem item, string key, TimeSpan expirationTime)
        {
            _memoryCache.Set(key, item, expirationTime);
        }

        public void Add<TItem>(TItem item, string key, DateTime absoluteExpiration)
        {
            _memoryCache.Set(key, item, absoluteExpiration);
        }

        public void Add<TItem>(TItem item, string key, MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, item, options);
        }

        public bool Clear()
        {
            GetKeys().ToList().ForEach( key => Remove(key));
            return true;
        }

        public IEnumerable<string> GetKeys()
        {
            return _memoryCache.GetKeys<string>();
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public bool TryGet<TItem>(string key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }
    }
}
