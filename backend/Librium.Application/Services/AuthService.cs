using Librium.Application.Security;
using Librium.Application.Services.Identity;
using Librium.Domain.Common;
using Librium.Domain.Users.DTOs;
using Librium.Domain.Users.Models;
using Librium.Domain.Users.Services;

namespace Librium.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtTokenService _tokenService;
    private readonly IIdentityService _identityService;

    public AuthService(IJwtTokenService tokenService, IIdentityService identityService)
    {
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<ValueOrResult<string>> Login(LoginDto dto)
    {
        var userId = await _identityService.ValidateCredential(dto.Email, dto.Password);
        if (!userId.IsSuccess)
            return ValueOrResult<string>.Failure(userId.ErrorMessage);

        var roles = await _identityService.GetRolesAsync(userId.Value);
        if (!roles.IsSuccess)
            return ValueOrResult<string>.Failure(roles.ErrorMessage);

        var token = _tokenService.CreateToken(userId.Value, dto.Email, roles.Value);

        return ValueOrResult<string>.Success(token);
    }

    public async Task<ValueOrResult> RegisterAdminAsync(RegisterDto dto)
    {
        var emailExists = await _identityService.EmailExistsAsync(dto.Email!);
        if (!emailExists.IsSuccess)
            return ValueOrResult.Failure(emailExists.ErrorMessage);

        var usernameExists = await _identityService.UsernameExistsAsync(dto.Username!);
        if (!usernameExists.IsSuccess)
            return ValueOrResult.Failure(usernameExists.ErrorMessage);

        var createResult = await _identityService.CreateUserAsync(dto.Email, dto.Username, dto.Password, UserRoles.Admin);
        if (!createResult.IsSuccess)
            return ValueOrResult.Failure(createResult.ErrorMessage);

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult> RegisterUserAsync(RegisterDto dto)
    {
        var emailExists = await _identityService.EmailExistsAsync(dto.Email!);
        if (!emailExists.IsSuccess)
            return ValueOrResult.Failure(emailExists.ErrorMessage);

        var usernameExists = await _identityService.UsernameExistsAsync(dto.Username!);
        if (!usernameExists.IsSuccess)
            return ValueOrResult.Failure(usernameExists.ErrorMessage);

        var createResult = await _identityService.CreateUserAsync(dto.Email, dto.Username, dto.Password!, UserRoles.User);
        if (!createResult.IsSuccess)
            return ValueOrResult.Failure(createResult.ErrorMessage);

        return ValueOrResult.Success();
    }
}
