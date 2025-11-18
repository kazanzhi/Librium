using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.DTOs.Books;

namespace Librium.Domain.Interfaces;
public interface IBookService
{
    Task<List<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto> GetBookById(Guid bookId);
    Task<ValueOrResult<Guid>> AddBookAsync(BookDto bookDto);
    Task<ValueOrResult> DeleteBookAsync(Guid bookId);
    Task<ValueOrResult> UpdateBookAsync(Guid bookId, BookDto bookDto);
}