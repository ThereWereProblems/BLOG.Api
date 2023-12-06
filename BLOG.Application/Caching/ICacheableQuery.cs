using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Caching
{
    public interface ICacheableQuery
    {
        bool BypassCache { get; set; }
        string CacheKey { get; }
        string CacheGroup { get; }
    }
}
