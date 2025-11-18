using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Librium.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    public IdentityService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    public async Task<ValueOrResult> CreateUserAsync(AppUser user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return ValueOrResult.Failure(result.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, role);

        return ValueOrResult.Success();
    }

    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AppUser?> FindByNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name);
    }

    public async Task<IList<string>> GetRolesAsync(AppUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}
