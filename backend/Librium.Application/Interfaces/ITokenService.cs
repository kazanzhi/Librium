using Librium.Domain.Entities;

namespace Librium.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
