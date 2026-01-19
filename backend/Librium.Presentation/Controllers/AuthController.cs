using Librium.Application.Abstractions.Auth;
using Librium.Application.DTOs.Auth;
using Librium.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterUserAsync(registerDto);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    [HttpPost("register-admin")]
    //[Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAdminAsync(registerDto);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(new { token = result.Value });
    }
}
