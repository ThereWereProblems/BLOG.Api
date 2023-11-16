using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.Model.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Nazwa użykownika
        /// </summary>
        public string NickName { get; set; }

        public virtual ICollection<Domain.Model.Post.Post> Posts { get; set; }
    }
}
