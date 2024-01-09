using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.Model.Comment
{
    public class Comment
    {
        /// <summary>
        /// Klucz główny
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Treść komentarza
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Data publikacji
        /// </summary>
        public DateTime PublishedAt { get; set; }
        /// <summary>
        /// Klucz główny wpisu
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// Klucz główny użytkownika komentującego
        /// </summary>
        public string UserId { get; set; }

        public virtual Domain.Model.Post.Post Post { get; set; }
        public virtual Domain.Model.ApplicationUser.ApplicationUser User { get; set; }
    }
}
