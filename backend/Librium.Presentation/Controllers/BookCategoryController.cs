using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Services;
using Librium.Domain.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _service;

    public BookCategoryController(IBookCategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] BookCategoryDto categoryDto)
    {
        var result = await _service.CreateBookCategoryAsync(categoryDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { Id = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetById(Guid Id)
    {
        var category = await _service.GetBookCategoryById(Id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var result = await _service.DeleteBookCategoryAsync(Id);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBookCategoriesAsync();
        return Ok(result);
    }

    [HttpPut("{Id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(Guid Id, [FromBody] BookCategoryDto categoryDto)
    {
        var result = await _service.UpdateBookCategoryAsync(Id, categoryDto);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }
}