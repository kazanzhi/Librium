using Librium.Domain.Books;

namespace Librium.Application.Libraries.Repositories;

public interface IUserBookRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid bookId);
    Task Remove(Guid userId, Guid bookId);
    void Add(Guid userId, Guid bookId);
    Task<IReadOnlyList<Book>> GetBooksByUserIdAsync(Guid userId);
    Task<bool> SaveChangesAsync();
}
