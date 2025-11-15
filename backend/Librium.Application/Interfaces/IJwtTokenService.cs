using Librium.Domain.Users.Models;

namespace Librium.Application.Interfaces;
public interface IJwtTokenService
{
    Task<string> CreateToken(AppUser user, IList<string> roles);
}
