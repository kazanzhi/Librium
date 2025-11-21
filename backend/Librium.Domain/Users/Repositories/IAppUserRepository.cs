using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Repositories;

public interface IAppUserRepository
{
    Task<List<UserBook>> GetAppUserBooks(string userId);
    Task<AppUser?> GetAppUserById(string userId);
    Task<bool> SaveChanges();
}
