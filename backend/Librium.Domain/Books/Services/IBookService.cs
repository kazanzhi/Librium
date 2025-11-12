using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Common;

namespace Librium.Domain.Interfaces;
public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();

    Task<ValueOrResult<int>> AddBookAsync(BookDto bookDto);
    Task<ValueOrResult> DeleteBookAsync(int bookId);
    Task<ValueOrResult> UpdateBookAsync(int bookId, BookDto bookDto);
}