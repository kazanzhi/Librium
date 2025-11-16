using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.Books;
using Librium.Domain.Common;

namespace Librium.Application.Interfaces;
public interface IBookService
{
    Task<List<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto> GetBookById(Guid bookId);
    Task<ValueOrResult<Guid>> AddBookAsync(BookDto bookDto);
    Task<ValueOrResult> DeleteBookAsync(Guid bookId);
    Task<ValueOrResult> UpdateBookAsync(Guid bookId, BookDto bookDto);
}