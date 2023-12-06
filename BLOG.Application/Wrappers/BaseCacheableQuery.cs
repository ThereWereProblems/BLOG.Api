using BLOG.Application.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Wrappers
{
    public abstract class BaseCacheableQuery<TResponse> : BaseQuery<TResponse>, ICacheableQuery
    {
        public bool BypassCache { get; set; } = false;

        public abstract string CacheKey { get;}

        public abstract string CacheGroup { get; }
    }
}
