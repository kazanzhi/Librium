namespace Librium.Application.Security;

public interface IJwtTokenService
{
    string CreateToken(Guid userId, string email, IList<string> roles);
}
