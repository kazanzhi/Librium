using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.Books;
using Librium.Domain.Common;

namespace Librium.Application.Interfaces;
public interface IBookService
{
    Task<List<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto> GetBookById(int bookId);
    Task<ValueOrResult<int>> AddBookAsync(BookDto bookDto);
    Task<ValueOrResult> DeleteBookAsync(int bookId);
    Task<ValueOrResult> UpdateBookAsync(int bookId, BookDto bookDto);
}