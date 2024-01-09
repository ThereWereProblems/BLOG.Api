using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.ReadModel
{
    public class CommentSearchResult
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
        /// Użytkownik tworzący
        /// </summary>
        public string User { get; set; }
    }
}
