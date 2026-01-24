using Librium.Application.Categories.Commands.CreateCategory;
using Librium.Application.Categories.Commands.DeleteCategory;
using Librium.Application.Categories.Commands.UpdateCategory;
using Librium.Application.Categories.DTOs;
using Librium.Application.Categories.Queries.GetAllCategories;
using Librium.Application.Categories.Queries.GetCategoryById;
using Librium.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        var result = await _sender.Send(new CreateCategoryCommand(dto));

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { Id = result.Value }, result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet("{Id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid Id)
    {
        var category = await _sender.Send(new GetCategoryByIdQuery(Id));

        return category is null 
            ? NotFound() 
            : Ok(category);
    }

    [HttpDelete("{Id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var result = await _sender.Send(new DeleteCategoryCommand(Id));

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllCategoriesQuery());

        return Ok(result);
    }

    [HttpPut("{Id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(Guid Id, [FromBody] CategoryDto dto)
    {
        var result = await _sender.Send(new UpdateCategoryCommand(Id, dto));

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.ErrorMessage);
    }
}