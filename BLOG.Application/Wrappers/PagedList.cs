using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Wrappers
{
    public class PagedList<TResponse>
    {
        public PagedList(DataPager dataPager, List<TResponse> result)
        {
            DataPager = dataPager;
            Result = result;
        }
        public virtual DataPager DataPager { get; set; }
        public virtual List<TResponse> Result { get; set; }
    }
}
