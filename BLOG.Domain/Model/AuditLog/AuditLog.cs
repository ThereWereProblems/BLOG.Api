using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Domain.Model.AuditLog
{
    public class AuditLog
    {
        /// <summary>
        /// Klucz główny
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Użytkownik dokonujący zmian
        /// </summary>
        public string? User { get; set; }
        /// <summary>
        /// Tabela
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// Typ akcji (Create, Update, Delete)
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Data zmian
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Dokonane zmiany
        /// </summary>
        public string? Changes { get; set; }
    }
}
