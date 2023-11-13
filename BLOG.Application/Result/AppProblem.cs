using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public record AppProblem(string title, AppResultStatus status);
}
