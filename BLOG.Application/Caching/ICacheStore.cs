using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Caching
{
    public interface ICacheStore
    {
        void Add<TItem>(TItem item, string key, TimeSpan expirationTime);
        void Add<TItem>(TItem item, string key, DateTime absoluteExpiration);
        void Add<TItem>(TItem item, string key, MemoryCacheEntryOptions options);
        bool TryGet<TItem>(string key, out TItem value);
        void Remove(string key);
        bool Clear();
        IEnumerable<string> GetKeys();
    }
}
