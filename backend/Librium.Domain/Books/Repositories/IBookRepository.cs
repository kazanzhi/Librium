using Librium.Domain.Books.Models;
using Librium.Domain.Common.Repositories;

namespace Librium.Domain.Books.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book?> GetBookById(Guid bookId);
    Task<List<Book>> GetAllBooks(string? search);
    Task<bool> ExistBookAsync(string author, string title);

    Task<IReadOnlyCollection<Book>> GetByIdsAsync(IEnumerable<Guid> bookIds);

    Task<Book> AddBook(Book book);
}