using Librium.Domain.Common.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetUserWithBooks(string userId);
    Task SaveChangesAsync();
}
