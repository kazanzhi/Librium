using Librium.Domain.Common;
using Librium.Domain.Users.DTOs;

namespace Librium.Domain.Users.Services;

public interface IAuthService
{
    Task<ValueOrResult<string>> Login(LoginDto loginDto);
    Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto);
    Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto);
}
