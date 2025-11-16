using Librium.Domain.Books.Models;
using Librium.Domain.Common;

namespace Librium.Domain.Users.Models;
public class UserBook
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime AddedAt { get; set; }

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
