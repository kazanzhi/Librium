using Librium.Domain.Interfaces;
using Librium.Domain.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Librium.Presentation.Controllers;

[Authorize(Roles = UserRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class UserBookController : ControllerBase
{
    private readonly IUserBookService _userBookService;
    public UserBookController(IUserBookService userBookService)
    {
        _userBookService = userBookService;
    }

    [HttpPost("{Id}")]
    public async Task<IActionResult> AddBook(Guid Id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var result = await _userBookService.AddUserBookAsync(userId, Id);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var books = await _userBookService.GetUserBooksAsync(userId);
        return Ok(books);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteBook(Guid Id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var result = await _userBookService.RemoveUserBookAsync(userId, Id);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
