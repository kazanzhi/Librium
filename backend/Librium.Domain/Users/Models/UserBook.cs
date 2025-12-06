using Librium.Domain.Books.Models;
using Librium.Domain.Common;

namespace Librium.Domain.Users.Models;

public class UserBook
{
    private UserBook() { }
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Book Book { get; private set; } = null!;
    public string UserId { get; private set; } = string.Empty;
    public AppUser AppUser { get; private set; } = null!;
    public DateTime AddedAt { get; private set; }

    public static ValueOrResult<UserBook> Create(string userId, Guid bookId)
    {
        if (bookId == Guid.Empty)
            return ValueOrResult<UserBook>.Failure("BookId must be valid.");

        if (string.IsNullOrWhiteSpace(userId))
            return ValueOrResult<UserBook>.Failure("UserId is required");

        var userBook = new UserBook
        {
            Id = Guid.NewGuid(),
            BookId = bookId,
            UserId = userId,
            AddedAt = DateTime.UtcNow
        };


        return ValueOrResult<UserBook>.Success(userBook);
    }
}
