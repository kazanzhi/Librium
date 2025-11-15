using Librium.Application.Interfaces;
using Librium.Domain.Common;
using Librium.Domain.Users.DTOs;
using Librium.Domain.Users.Models;

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
    public async Task<ValueOrResult<string>> Login(LoginDto loginDto)
    {
        var user = await _identityService.FindByEmailAsync(loginDto.Email);
        if (user is null)
            return ValueOrResult<string>.Failure("Invalid email or password.");

        var validPassword = await _identityService.CheckPasswordAsync(user, loginDto.Password);
        if (!validPassword)
            return ValueOrResult<string>.Failure("Invalid email or password.");

        var roles = await _identityService.GetRolesAsync(user);
        var token = await _tokenService.CreateToken(user, roles);

        return ValueOrResult<string>.Success(token);
    }

    public async Task<ValueOrResult> RegisterAdminAsync(RegisterDto registerDto)
    {
        var emailExists = await _identityService.FindByEmailAsync(registerDto.Email);
        if (emailExists is not null)
            return ValueOrResult.Failure("User with this email already exists.");

        var nameExists = await _identityService.FindByNameAsync(registerDto.Username);
        if (nameExists is not null)
            return ValueOrResult.Failure("User with this username already exists.");

        var newUser = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var createResult = await _identityService.CreateUserAsync(newUser, registerDto.Password, Roles.Admin);
        if (!createResult.isSuccess)
            return ValueOrResult.Failure(createResult.ErrorMessage!);

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult> RegisterUserAsync(RegisterDto registerDto)
    {
        var emailExists = await _identityService.FindByEmailAsync(registerDto.Email);
        if (emailExists is not null)
            return ValueOrResult.Failure("User with this email already exists.");

        var nameExists = await _identityService.FindByNameAsync(registerDto.Username);
        if (nameExists is not null)
            return ValueOrResult.Failure("User with this username already exists.");

        var newUser = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var createResult = await _identityService.CreateUserAsync(newUser, registerDto.Password, Roles.User);
        if (!createResult.isSuccess)
            return ValueOrResult.Failure(createResult.ErrorMessage!);

        return ValueOrResult.Success();
    }
}
