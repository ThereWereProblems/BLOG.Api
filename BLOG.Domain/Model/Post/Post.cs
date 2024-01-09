using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.Model.Post
{
    public class Post
    {
        /// <summary>
        /// Klucz
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// Nazwa zdjęcia na miniaturkę
        /// </summary>
        public string Image{ get; set; }
        /// <summary>
        /// Data publikacji
        /// </summary>
        public DateTime PublishedAt { get; set; }
        /// <summary>
        /// Klucz do użytkownika tworzącego
        /// </summary>
        public string? UserId { get; set; }

        public virtual Domain.Model.ApplicationUser.ApplicationUser User { get; set; }
        public virtual ICollection<Domain.Model.Comment.Comment> Comments { get; set; }
    }
}
