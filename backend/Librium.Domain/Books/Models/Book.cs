using Librium.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Librium.Domain.Books.Models;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int CategoryId { get; set; }
    public BookCategory BookCategory { get; set; }
    public string Content { get; set; }
    public int PublishedYear { get; set; }

    [JsonIgnore]
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}
