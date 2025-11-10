using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Librium.Domain.Entities.Books
{
    public class BookCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
