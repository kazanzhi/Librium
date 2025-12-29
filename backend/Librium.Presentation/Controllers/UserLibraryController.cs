using Librium.Domain.Users.Models;
using Librium.Domain.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Librium.Presentation.Controllers;

[Authorize(Roles = UserRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class UserLibraryController : ControllerBase
{
    private readonly IUserLibraryService _libraryService;

    public UserLibraryController(IUserLibraryService libraryService)
    {
        _libraryService = libraryService;
    }
    private Guid GetUserId()
    {
        return Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }

    [HttpPost("{bookId:guid}")]
    public async Task<IActionResult> AddBook(Guid bookId)
    {
        var userId = GetUserId();

        var result = await _libraryService.AddBookAsync(userId, bookId);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyLibrary()
    {
        var userId = GetUserId();

        var result = await _libraryService.GetUserLibraryAsync(userId);

        return Ok(result);
    }

    [HttpDelete("{bookId:guid}")]
    public async Task<IActionResult> RemoveBook(Guid bookId)
    {
        var userId = GetUserId();

        var result = await _libraryService.RemoveBookAsync(userId, bookId);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
