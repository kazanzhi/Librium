using Librium.Application.DTOs.Books;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Services;

public interface IBookService
{
    Task<List<BookResponseDto>> GetAllBooksAsync(string? search);
    Task<ValueOrResult<BookResponseDto>> GetBookById(Guid bookId);
    Task<ValueOrResult<Guid>> CreateBookAsync(BookDto bookDto);
    Task<ValueOrResult> DeleteBookAsync(Guid bookId);
    Task<ValueOrResult> UpdateBookAsync(Guid bookId, BookDto bookDto);
    Task<ValueOrResult> AddCategoryToBook(Guid bookId, string category);
    Task<ValueOrResult> RemoveCategoryFromBook(Guid bookId, string category);
}
