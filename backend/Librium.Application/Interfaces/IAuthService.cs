using Librium.Application.DTOs.Users;
using Librium.Domain.Common;

namespace Librium.Application.Interfaces;

public interface IAuthService
{
    Task<ValueOrResult<string>> Login(LoginDto loginDto);
    Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto);
    Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto);
}
