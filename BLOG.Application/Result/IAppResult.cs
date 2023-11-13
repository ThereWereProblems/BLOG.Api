using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public interface IAppResult
    {
        AppProblem? Problem { get; }
        IEnumerable<AppProblemDetail> Errors { get; }
        Type? ValueType { get; }
        object? GetValue();
    }
}
