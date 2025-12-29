using Librium.Domain.Common;
using Librium.Domain.DTOs.Books;
using Librium.Domain.Users.DTOs;

namespace Librium.Domain.Users.Services;

public interface IUserLibraryService
{
    Task<List<BookResponseDto>> GetUserLibraryAsync(Guid userId);
    Task<ValueOrResult> AddBookAsync(Guid userId, Guid bookId);
    Task<ValueOrResult> RemoveBookAsync(Guid userId, Guid bookId);
}
