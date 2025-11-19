using Microsoft.AspNetCore.Identity;

namespace Librium.Domain.Users.Models;

public class AppUser : IdentityUser
{
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}
