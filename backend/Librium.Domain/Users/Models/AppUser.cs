using Librium.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Librium.Domain.Users.Models;

public class AppUser : IdentityUser
{
    public ICollection<UserBook> UserBooks { get; private set; } = new List<UserBook>();

    public ValueOrResult AddBook(Guid bookId)
    {
        if (UserBooks.Any(x => x.BookId == bookId))
            return ValueOrResult<UserBook>.Failure("Book already in the library.");

        var result = UserBook.Create(Id, bookId);
        if (!result.IsSuccess || result.Value is null)
            return ValueOrResult<UserBook>.Failure(result.ErrorMessage!);

        UserBooks.Add(result.Value);

        return ValueOrResult.Success();
    }

    public ValueOrResult RemoveBook(Guid bookId)
    {
        var book = UserBooks.FirstOrDefault(b => b.BookId == bookId);
        if (book is null)
            return ValueOrResult<UserBook>.Failure("Book is not in user's library.");

        UserBooks.Remove(book);
            
        return ValueOrResult.Success();
    }
}
