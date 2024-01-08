using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.ReadModel
{
    public class PostSearchResult
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
        /// Nazwa zdjęcia na miniaturkę
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Data publikacji
        /// </summary>
        public DateTime PublishedAt { get; set; }
        /// <summary>
        /// Osoba publikująca
        /// </summary>
        public string Author { get; set; }
    }
}
