using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Repositories;

public interface IUserLibraryRepository
{
    Task<UserLibrary?> GetByUserIdAsync(Guid userId);
    Task Add(UserLibrary userLibrary);
    Task<bool> SaveChanges();
}
