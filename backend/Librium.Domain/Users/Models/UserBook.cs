using Librium.Domain.Books.Models;
using Librium.Domain.Common;

namespace Librium.Domain.Users.Models;
public class UserBook
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime AddedAt { get; set; }

    public static ValueOrResult<UserBook> Create(string userId, int bookId)
    {
        if (bookId <= 0)
            return ValueOrResult<UserBook>.Failure("BookId must be valid.");

        if (string.IsNullOrWhiteSpace(userId))
            return ValueOrResult<UserBook>.Failure("UserId is required");

        var userBook = new UserBook
        {
            BookId = bookId,
            UserId = userId,
            AddedAt = DateTime.UtcNow
        };

        return ValueOrResult<UserBook>.Success(userBook);
    }
}
