using Librium.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Librium.Domain.Users.Models;

public class AppUser : IdentityUser
{
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public ValueOrResult<UserBook> AddBook(Guid bookId)
    {
        if (UserBooks.Any(x => x.BookId == bookId))
            return ValueOrResult<UserBook>.Failure("Book already in the library.");

        var result = UserBook.Create(Id, bookId);
        if (!result.IsSuccess)
            return ValueOrResult<UserBook>.Failure(result.ErrorMessage!);

        return ValueOrResult<UserBook>.Success(result.Value!);
    }

    public ValueOrResult<UserBook> RemoveBook(Guid bookId)
    {
        var ub = UserBooks.FirstOrDefault(b => b.BookId == bookId);
        if (ub is null)
            return ValueOrResult<UserBook>.Failure("Book is not in user's library.");

        return ValueOrResult<UserBook>.Success(ub);
    }
}
