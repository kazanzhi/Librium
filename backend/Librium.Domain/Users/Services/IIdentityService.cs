using Librium.Domain.Common;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Services;

public interface IIdentityService
{
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<AppUser?> FindByNameAsync(string name);
    Task<IList<string>> GetRolesAsync(AppUser user);
    Task<ValueOrResult> CreateUserAsync(AppUser user, string password, string role);
}