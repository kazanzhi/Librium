using Librium.Domain.Books.DTOs;
using Librium.Domain.Interfaces;
using Librium.Domain.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBooksAsync();
        return Ok(result);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid Id)
    {
        var result = await _service.GetBookById(Id);
        return result == null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] BookDto bookDto)
    {
        var result = await _service.AddBookAsync(bookDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { Id = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var result = await _service.DeleteBookAsync(Id);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpPut("{Id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(Guid Id, [FromBody] BookDto bookDto)
    {
        var result = await _service.UpdateBookAsync(Id, bookDto);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
