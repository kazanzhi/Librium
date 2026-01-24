using Librium.Application.Abstractions.Auth.DTOs;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Auth;

public interface IAuthService
{
    Task<ValueOrResult<string>> Login(LoginDto loginDto);
    Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto);
    Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto);
}
