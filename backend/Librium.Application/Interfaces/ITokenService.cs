using Librium.Domain.Users.Models;

namespace Librium.Application.Interfaces;
public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}
