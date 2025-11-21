using Librium.Domain.Common.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Repositories;

public interface IAppUserRepository : IBaseRepository<UserBook>
{
    Task<List<UserBook>> GetAppUserBooks(string userId);
    Task<AppUser?> GetAppUserById(string userId);
    Task Add(UserBook userBook);
}
