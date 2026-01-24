using Librium.Application.Libraries.Commands.AddBookToUserLibrary;
using Librium.Application.Libraries.Commands.RemoveBookFromUserLibrary;
using Librium.Application.Libraries.Queries.GetUserLibrary;
using Librium.Identity.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Librium.Presentation.Controllers;

[Authorize(Roles = UserRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class UserLibraryController : ControllerBase
{
    private readonly ISender _sender;

    public UserLibraryController(ISender sender)
    {
        _sender = sender;
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

        var result = await _sender.Send(new AddBookToLibraryCommand(userId, bookId));

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserLibrary()
    {
        var userId = GetUserId();

        var result = await _sender.Send(new GetUserLibraryQuery(userId));

        return Ok(result);
    }

    [HttpDelete("{bookId:guid}")]
    public async Task<IActionResult> RemoveBook(Guid bookId)
    {
        var userId = GetUserId();

        var result = await _sender.Send(new RemoveBookFromUserLibraryCommand(userId, bookId));

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
