using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public class AppProblems
    {
        public static AppProblem Error => new(
            "Error",
            AppResultStatus.Error);

        public static AppProblem Forbidden => new(
            "Forbidden",
            AppResultStatus.Forbidden);

        public static AppProblem Unathorized => new(
            "Unathorized",
            AppResultStatus.Unathorized);

        public static AppProblem Invalid => new(
            "Invalid",
            AppResultStatus.Invalid);

        public static AppProblem NotFound => new(
            "NotFound",
            AppResultStatus.NotFound);

        public static AppProblem Conflict => new(
            "Conflict",
            AppResultStatus.Conflict);
    }
}
