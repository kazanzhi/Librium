using Librium.Application.Abstractions.Services;
using Librium.Application.DTOs.Books;
using Librium.Application.DTOs.Categories;
using Librium.Identity.Authorization;
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
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _service.GetAllBooksAsync(search);
        return Ok(result);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid Id)
    {
        var result = await _service.GetBookById(Id);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.ErrorMessage);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] BookDto bookDto)
    {
        var result = await _service.CreateBookAsync(bookDto);
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

    [HttpPost("{bookId}/categories")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AddCategory(Guid bookId, [FromBody] CategoryDto categoryDto)
    {
        var result = await _service.AddCategoryToBook(bookId, categoryDto.Name!);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{bookId}/categories/{categoryName}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> RemoveCategory(Guid bookId, string categoryName)
    {
        var result = await _service.RemoveCategoryFromBook(bookId, categoryName);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}
