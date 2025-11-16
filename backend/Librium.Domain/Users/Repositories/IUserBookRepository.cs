using Librium.Domain.Common.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Repositories;
public interface IUserBookRepository : IBaseRepository<UserBook>
{
    Task<UserBook?> GetUserBookById(string userId, Guid bookId);
    Task<List<UserBook>> GetAllUserBooks(string userId);
    Task<UserBook> AddUserBook(UserBook userBook);
}
