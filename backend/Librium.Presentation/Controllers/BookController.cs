using Librium.Domain.Books.DTOs;
using Librium.Domain.Interfaces;
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
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBooksAsync();
        return Ok(result);
    }

    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetById(int bookId)
    {
        var result = await _service.GetBookById(bookId);
        return result == null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookDto bookDto)
    {
        var result = await _service.AddBookAsync(bookDto);
        return result.isSuccess
            ? CreatedAtAction(nameof(GetById), new { bookId = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> Delete(int bookId)
    {
        var result = await _service.DeleteBookAsync(bookId);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> Update(int bookId, BookDto bookDto)
    {
        var result = await _service.UpdateBookAsync(bookId, bookDto);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
