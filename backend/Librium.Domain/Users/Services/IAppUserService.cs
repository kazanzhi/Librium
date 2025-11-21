using Librium.Domain.Common;
using Librium.Domain.Users.DTOs;

namespace Librium.Domain.Interfaces;

public interface IAppUserService
{
    Task<List<UserBookResponseDto>> GetUserBooksAsync(string userId);
    Task<ValueOrResult> AddUserBookAsync(string userId, Guid bookId);
    Task<ValueOrResult> RemoveUserBookAsync(string userId, Guid bookId);
}
