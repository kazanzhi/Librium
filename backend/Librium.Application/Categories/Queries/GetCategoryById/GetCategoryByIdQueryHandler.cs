using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ValueOrResult<CategoryResponseDto>>
{
    private readonly ICategoryRepository _repo;
    public GetCategoryByIdQueryHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<ValueOrResult<CategoryResponseDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetCategoryByIdAsync(request.Id);

        if (category is null)
            return ValueOrResult<CategoryResponseDto>.Failure("Category not found.");

        return ValueOrResult<CategoryResponseDto>.Success(new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}
