using Librium.Domain.Books.Dtos;
using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;

namespace Librium.Domain.Repositories;
public interface IBookRepository
{
    Task<List<Book>> GetBooks();
    Task<Book> GetBook(int bookId);

    Task<Book> CreateBook(BookDto bookDto);
    Task<int> DeleteBook(int bookId);
    Task<int> UpdateBook(int bookId, BookDto bookDto);
}
