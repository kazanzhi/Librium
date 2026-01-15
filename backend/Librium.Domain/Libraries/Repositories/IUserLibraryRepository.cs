namespace Librium.Domain.Libraries.Repositories;

public interface IUserLibraryRepository
{
    Task<UserLibrary?> GetByUserIdAsync(Guid userId);
    Task Add(UserLibrary userLibrary);
    Task<bool> SaveChanges();
}
