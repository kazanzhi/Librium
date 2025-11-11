using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Users.DTOs;
public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(6, ErrorMessage = "Username must be at least 6 characters long")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{6,}$",
        ErrorMessage = "Password must contain at least one uppercase, one lowercase, one number, and one special character")]
    public string Password { get; set; }
}
