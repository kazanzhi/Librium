using Librium.Application.Interfaces;
using Librium.Domain.Users.DTOs;
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
        if (!result.isSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAdminAsync(registerDto);
        if (!result.isSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        if (!result.isSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Value);
    }
}
