using Librium.Domain.Common;

namespace Librium.Domain.Books.Models;

public class BookCategory
{
    private BookCategory() { }
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ICollection<Book> Books { get; private set; } = new List<Book>();

    public static ValueOrResult<BookCategory> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult<BookCategory>.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<BookCategory>.Failure("Category name cannot exceed 100 characters.");

        var category = new BookCategory
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };

        return ValueOrResult<BookCategory>.Success(category);
    }

    public ValueOrResult Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<BookCategory>.Failure("Category name cannot exceed 100 characters.");

        Name = name.Trim();

        return ValueOrResult.Success();
    }
}
