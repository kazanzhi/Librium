using Librium.Domain.Common;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Interfaces;
public interface IUserBookService
{
    Task<List<UserBook>> GetUserBooksAsync(string userId);
    Task<ValueOrResult> AddUserBookAsync(string userId, int bookId);
    Task<ValueOrResult> RemoveUserBookAsync(string userId, int bookId);
}
