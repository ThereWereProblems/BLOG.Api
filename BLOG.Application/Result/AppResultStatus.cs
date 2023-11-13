using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public enum AppResultStatus
    {
        Error,
        Forbidden,
        Unathorized,
        Invalid,
        NotFound,
        Conflict
    }
}
