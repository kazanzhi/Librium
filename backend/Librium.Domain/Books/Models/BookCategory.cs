using Librium.Domain.Books.Models;
using Librium.Domain.Common;

namespace Librium.Domain.Entities.Books;

public class BookCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();

    public static ValueOrResult<BookCategory> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult<BookCategory>.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<BookCategory>.Failure("Category name cannot exceed 100 characters.");

        var category = new BookCategory
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        return ValueOrResult<BookCategory>.Success(category);
    }
}
