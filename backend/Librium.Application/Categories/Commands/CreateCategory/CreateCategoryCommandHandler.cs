using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ValueOrResult<Guid>>
{
    private readonly ICategoryRepository _repo;
    public CreateCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }
    public async Task<ValueOrResult<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Dto.Name!.Trim();
        var exists = await _repo.GetByNameAsync(name);
        if (exists is not null)
            return ValueOrResult<Guid>.Failure("This category already exists.");

        var categoryResult = Category.Create(name);
        if (!categoryResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(categoryResult.ErrorMessage!);

        var category = categoryResult.Value;

        await _repo.AddBookCategory(category);
        await _repo.SaveChanges();

        return ValueOrResult<Guid>.Success(category.Id);
    }
}