using Librium.Domain.Common;

namespace Librium.Identity.Contracts;

public interface IIdentityService
{
    Task<ValueOrResult<Guid>> ValidateCredential(string email, string password);
    Task<ValueOrResult> EmailExistsAsync(string email);
    Task<ValueOrResult> UsernameExistsAsync(string name);
    Task<ValueOrResult<IList<string>>> GetRolesAsync(Guid userId);
    Task<ValueOrResult<Guid>> CreateUserAsync(string email, string username, string password, string role);
}
