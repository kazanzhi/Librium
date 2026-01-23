using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using MediatR;

namespace Librium.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponseDto>
{
    private readonly ICategoryRepository _repo;
    public GetCategoryByIdQueryHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<CategoryResponseDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetBookCategoryByIdAsync(request.Id);

        return new CategoryResponseDto
        {
            Id = category!.Id,
            Name = category.Name
        };
    }
}
