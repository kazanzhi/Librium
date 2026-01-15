using Librium.Domain.Common;
using Librium.Identity.Contracts;
using Librium.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Librium.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    public IdentityService(UserManager<AppIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ValueOrResult<Guid>> CreateUserAsync(string email, string username, string password, string role)
    {
        var user = new AppIdentityUser
        {
            Email = email,
            UserName = username
        };

        var valid = await _userManager.CreateAsync(user, password);
        if (!valid.Succeeded)
            return ValueOrResult<Guid>.Failure(valid.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, role);

        return ValueOrResult<Guid>.Success(user.Id);
    }

    public async Task<ValueOrResult> EmailExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
            return ValueOrResult.Failure("A user with this email already exists.");

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult> UsernameExistsAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user is not null)
            return ValueOrResult.Failure("A user with this username already exists.");

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult<IList<string>>> GetRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return ValueOrResult<IList<string>>.Failure("User not found.");

        var roles = await _userManager.GetRolesAsync(user);

        return ValueOrResult<IList<string>>.Success(roles);
    }

    public async Task<ValueOrResult<Guid>> ValidateCredential(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return ValueOrResult<Guid>.Failure("Invalid email or password.");

        var valid = await _userManager.CheckPasswordAsync(user, password);
        if (!valid)
            return ValueOrResult<Guid>.Failure("Invalid email or password.");

        return ValueOrResult<Guid>.Success(user.Id);
    }
}
