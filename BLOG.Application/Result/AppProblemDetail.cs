using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public class AppProblemDetail
    {
        public string ErrorMessage { get; set; }

        public AppProblemDetail(string identyfikator, string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
