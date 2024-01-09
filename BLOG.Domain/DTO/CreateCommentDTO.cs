using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.DTO
{
    public class CreateCommentDTO
    {
        /// <summary>
        /// Treść komentarza
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Klucz główny wpisu
        /// </summary>
        public int PostId { get; set; }
    }
}
