using Librium.Domain.Common.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Repositories;
public interface IUserBookRepository : IBaseRepository
{
    Task<UserBook?> GetUserBookById(string userId, int bookId);
    Task<UserBook> AddUserBook(string userId, int bookId);
}
