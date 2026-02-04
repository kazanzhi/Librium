using Librium.Domain.Categories.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ValueOrResult>
{
    private readonly ICategoryRepository _repo;
    public UpdateCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<ValueOrResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetCategoryByIdAsync(request.CategoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        var updateResult = category.Update(request.Dto.Name!);
        if (!updateResult.IsSuccess)
            return ValueOrResult.Failure(updateResult.ErrorMessage!);

        await _repo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}