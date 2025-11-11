using Microsoft.AspNetCore.Identity;

namespace Librium.Domain.Users.Models;
public class AppUser : IdentityUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}
