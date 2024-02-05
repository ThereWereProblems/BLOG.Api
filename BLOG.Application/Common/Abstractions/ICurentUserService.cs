using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        /// <summary>
        /// Czy użytkownik jest adminem
        /// </summary>
        bool IsAdmin { get; }

        /// <summary>
        /// Lista roli użytkownika
        /// </summary>
        List<string> Roles { get; }
    }
}
