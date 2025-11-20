using Librium.Domain.Books.Models;
using Librium.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Librium.Domain.Users.Models;

public class AppUser : IdentityUser
{
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public ValueOrResult AddBook(Guid bookId)
    {
        if (UserBooks.Any(x => x.BookId == bookId))
            return ValueOrResult.Failure("Book already in the library.");

        var result = UserBook.Create(Id, bookId);
        if (!result.IsSuccess)
            return ValueOrResult.Failure(result.ErrorMessage!);

        UserBooks.Add(result.Value!);

        return ValueOrResult.Success();
    }

    public ValueOrResult RemoveBook(Guid bookId)
    {
        var ub = UserBooks.FirstOrDefault(b => b.BookId == bookId);
        if (ub is null)
            return ValueOrResult.Failure("Book is not in user's library.");

        UserBooks.Remove(ub);

        return ValueOrResult.Success();
    }
}
