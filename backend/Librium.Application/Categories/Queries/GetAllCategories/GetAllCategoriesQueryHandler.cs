using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories.Repositories;
using MediatR;

namespace Librium.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryResponseDto>>
{
    private readonly ICategoryRepository _repo;
    public GetAllCategoriesQueryHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }
    public async Task<IReadOnlyList<CategoryResponseDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repo.GetAllBookCategoriesAsync();

        return categories.Select(category => new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }
}
