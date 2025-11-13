using Librium.Domain.Books.DTOs;
using Librium.Domain.Interfaces;
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
    public async Task<IActionResult> Create(BookCategoryDto categoryDto)
    {
        var result = await _service.AddBookCategoryAsync(categoryDto);
        return result.isSuccess
            ? CreatedAtAction(nameof(GetById), new { categoryId = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetById(int categoryId)
    {
        var category = await _service.GetBookCategoryById(categoryId);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(int categoryId)
    {
        var result = await _service.DeleteBookCategoryAsync(categoryId);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBookCategoriesAsync();
        return Ok(result);
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> Update(int categoryId, BookCategoryDto categoryDto)
    {
        var result = await _service.UpdateBookCategoryAsync(categoryId, categoryDto);
        return result.isSuccess
            ? Ok()
            : BadRequest(result.ErrorMessage);
    }

}
