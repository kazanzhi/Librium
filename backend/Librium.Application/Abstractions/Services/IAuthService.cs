using Librium.Application.DTOs.Auth;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Services;

public interface IAuthService
{
    Task<ValueOrResult<string>> Login(LoginDto loginDto);
    Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto);
    Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto);
}
