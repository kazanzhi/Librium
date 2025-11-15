using Librium.Application.Books.DTOs;
using Librium.Application.Interfaces;
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

    [HttpGet("{bookId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int bookId)
    {
        var result = await _service.GetBookById(bookId);
        return result == null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] BookDto bookDto)
    {
        var result = await _service.AddBookAsync(bookDto);
        return result.isSuccess
            ? CreatedAtAction(nameof(GetById), new { bookId = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{bookId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int bookId)
    {
        var result = await _service.DeleteBookAsync(bookId);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpPut("{bookId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int bookId, [FromBody] BookDto bookDto)
    {
        var result = await _service.UpdateBookAsync(bookId, bookDto);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
