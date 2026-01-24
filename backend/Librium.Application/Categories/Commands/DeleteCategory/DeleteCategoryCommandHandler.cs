using Librium.Domain.Categories.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ValueOrResult>
{
    private readonly ICategoryRepository _repo;
    public DeleteCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;   
    }
    public async Task<ValueOrResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetBookCategoryByIdAsync(request.CategoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        _repo.Delete(category);
        await _repo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
