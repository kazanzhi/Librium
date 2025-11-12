using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Common.Repositories;

namespace Librium.Domain.Repositories;
public interface IBookRepository : IBaseRepository
{
    Task<Book?> GetBookById(int bookId); 
    Task<Book> AddBook(Book book);
}