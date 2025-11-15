using Librium.Application.Books.DTOs;
using Librium.Application.Interfaces;
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
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] BookCategoryDto categoryDto)
    {
        var result = await _service.AddBookCategoryAsync(categoryDto);
        return result.isSuccess
            ? CreatedAtAction(nameof(GetById), new { categoryId = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{categoryId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetById(int categoryId)
    {
        var category = await _service.GetBookCategoryById(categoryId);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpDelete("{categoryId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int categoryId)
    {
        var result = await _service.DeleteBookCategoryAsync(categoryId);
        return result.isSuccess
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

    [HttpPut("{categoryId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int categoryId, [FromBody] BookCategoryDto categoryDto)
    {
        var result = await _service.UpdateBookCategoryAsync(categoryId, categoryDto);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

}
