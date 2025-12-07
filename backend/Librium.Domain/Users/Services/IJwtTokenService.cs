using Librium.Domain.Users.Models;

namespace Librium.Domain.Users.Services;

public interface IJwtTokenService
{
    string CreateToken(AppUser user, IList<string> roles);
}
