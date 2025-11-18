using Librium.Domain.Common;
using Librium.Domain.DTOs.Users;

namespace Librium.Domain.Interfaces;

public interface IAuthService
{
    Task<ValueOrResult<string>> Login(LoginDto loginDto);
    Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto);
    Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto);
}
