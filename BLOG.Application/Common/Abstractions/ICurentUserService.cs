using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Common.Abstractions
{
    public interface ICurentUserService
    {
        /// <summary>
        /// Id użytkownika
        /// </summary>
        string UserId { get; }
    }
}
