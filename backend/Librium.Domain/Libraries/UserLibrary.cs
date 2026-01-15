using Librium.Domain.Common;

namespace Librium.Domain.Libraries;

public class UserLibrary
{
    private readonly List<LibraryBook> _books = new();
    private UserLibrary() { }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<LibraryBook> Books => _books;

    public static ValueOrResult<UserLibrary> Create(Guid userId)
    {
        if (userId == Guid.Empty)
            return ValueOrResult<UserLibrary>.Failure("UserId is required");

        return ValueOrResult<UserLibrary>.Success(new UserLibrary { UserId = userId });
    }

    public ValueOrResult AddBook(Guid bookId)
    {
        if (bookId == Guid.Empty)
            return ValueOrResult.Failure("BookId is required.");

        if (_books.Any(b => b.BookId == bookId))
            return ValueOrResult.Failure("Book already in library.");

        _books.Add(new LibraryBook(bookId));
        return ValueOrResult.Success();
    }

    public ValueOrResult RemoveBook(Guid bookId)
    {
        var existing = _books.FirstOrDefault(b => b.BookId == bookId);
        if (existing is null)
            return ValueOrResult.Failure("Book not in library.");

        _books.Remove(existing);
        return ValueOrResult.Success();
    }
}
