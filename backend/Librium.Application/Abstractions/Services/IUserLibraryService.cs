using Librium.Application.DTOs.Books;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Services;

public interface IUserLibraryService
{
    Task<List<BookResponseDto>> GetUserLibraryAsync(Guid userId);
    Task<ValueOrResult> AddBookAsync(Guid userId, Guid bookId);
    Task<ValueOrResult> RemoveBookAsync(Guid userId, Guid bookId);
}
