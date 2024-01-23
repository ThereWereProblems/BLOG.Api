using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.ReadModel
{
    public class UserInfoResult
    {
        /// <summary>
        /// Nazwa użykownika
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Uprawnienia
        /// </summary>
        public ICollection<string> Roles { get; set; }
    }
}
