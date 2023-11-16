using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.DTO
{
    public class CreatePostDTO
    {
        /// <summary>
        /// Tytuł
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Krótki opis
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Treść
        /// </summary>
        public string Content { get; set; }
    }
}
