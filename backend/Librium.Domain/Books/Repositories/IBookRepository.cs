using Librium.Domain.Books.Models;
using Librium.Domain.Common.Repositories;

namespace Librium.Domain.Books.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book?> GetBookById(Guid bookId);
    Task<List<Book>> GetAllBooks();
    Task<Book> AddBook(Book book);
}