using Librium.Application.Books.Commands.AddCategoryToBook;
using Librium.Application.Books.Commands.CreateBook;
using Librium.Application.Books.Commands.DeleteBook;
using Librium.Application.Books.Commands.RemoveCategoryFromBook;
using Librium.Application.Books.Commands.UpdateBook;
using Librium.Application.Books.DTOs;
using Librium.Application.Books.Queries.GetAllBooks;
using Librium.Application.Books.Queries.GetBookById;
using Librium.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly ISender _sender;

    public BookController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBooks([FromQuery] string? search)
    {
        var result = await _sender.Send(new GetAllBooksQuery(search));

        return Ok(result);
    }

    [HttpGet("{Id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookById(Guid Id)
    {
        var result = await _sender.Send(new GetBookByIdQuery(Id));

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.ErrorMessage);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
    {
        var result = await _sender.Send(new CreateBookCommand(bookDto));

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetBookById), new { Id = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{Id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteBook(Guid Id)
    {
        var result = await _sender.Send(new DeleteBookCommand(Id));

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }

    [HttpPut("{Id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateBook(Guid Id, [FromBody] BookDto bookDto)
    {
        var result = await _sender.Send(new UpdateBookCommand(Id, bookDto));

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }

    [HttpPost("{bookId:guid}/categories/{categoryId:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AddCategoryToBook(Guid bookId, Guid categoryId)
    {
        var result = await _sender.Send(new AddCategoryToBookCommand(bookId, categoryId));

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{bookId:guid}/categories/{categoryId:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> RemoveCategoryFromBook(Guid bookId, Guid categoryId)
    {
        var result = await _sender.Send(new RemoveCategoryFromBookCommand(bookId, categoryId));
        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }
}
