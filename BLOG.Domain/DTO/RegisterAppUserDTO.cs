using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.DTO
{
    public class RegisterAppUserDTO
    {
        /// <summary>
        /// Nazwa użykownika
        /// </summary>
        [Required]
        public string NickName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Hasło
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
