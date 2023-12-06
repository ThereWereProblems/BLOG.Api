using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Caching
{
    public class CacheConfiguration
    {
        /// <summary>
        /// Minuty wygaśnięcia buforu od ostatniego zapytania
        /// </summary>
        public int SlidingExpiration { get; set; }

        /// <summary>
        /// Minuty wygaśnięcia buforu od utworzenia 
        /// </summary>
        public int AbsoluteExpiration { get; set; }
    }
}
