using System.ComponentModel.DataAnnotations;

namespace Librium.Identity.Auth;

public record LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
