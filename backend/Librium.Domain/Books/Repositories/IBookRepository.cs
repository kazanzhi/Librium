using Librium.Domain.Common.Repositories;

namespace Librium.Domain.Books.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book?> GetBookById(Guid bookId);
    Task<IReadOnlyList<Book>> GetAllBooks(string? search);
    Task<bool> ExistBookAsync(string author, string title);
    Task<Book> AddBook(Book book);
}