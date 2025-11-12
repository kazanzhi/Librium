using Librium.Domain.Books.Models;
using Librium.Domain.Common;
using System.Text.Json.Serialization;

namespace Librium.Domain.Entities.Books;
public class BookCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = new List<Book>();

    private BookCategory(string name) => Name = name.Trim();

    public static ValueOrResult<BookCategory> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult<BookCategory>.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<BookCategory>.Failure("Category name cannot exceed 100 characters.");


        return ValueOrResult<BookCategory>.Success(new BookCategory(name));
    }
}
