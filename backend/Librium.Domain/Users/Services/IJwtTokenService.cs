using Librium.Domain.Users.Models;

namespace Librium.Domain.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateToken(AppUser user, IList<string> roles);
}
