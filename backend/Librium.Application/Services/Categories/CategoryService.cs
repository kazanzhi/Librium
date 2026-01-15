using Librium.Domain.Common;
using Librium.Application.Abstractions.Services;
using Librium.Application.DTOs.Categories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;

namespace Librium.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ValueOrResult<Guid>> CreateBookCategoryAsync(CategoryDto categoryDto)
    {
        var name = categoryDto.Name!.Trim();
        var categoryExists = await _repository.GetByNameAsync(name);
        if (categoryExists is not null)
            return ValueOrResult<Guid>.Failure("This category already exists.");
       
        var categoryResult = Category.Create(name);
        if (!categoryResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(categoryResult.ErrorMessage!);

        var category = categoryResult.Value;

        await _repository.AddBookCategory(category);
        await _repository.SaveChanges();

        return ValueOrResult<Guid>.Success(category.Id);
    }

    public async Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        _repository.Delete(category);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<CategoryResponseDto>> GetAllBookCategoriesAsync()
    {
        var categories = await _repository.GetAllBookCategoriesAsync();

        return categories.Select(category => new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }

    public async Task<CategoryResponseDto> GetBookCategoryById(Guid categoryId)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);

        return new CategoryResponseDto
        {
            Id = category!.Id,
            Name = category.Name
        };
    }

    public async Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, CategoryDto categoryDto)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        var updateResult = category.Update(categoryDto.Name!);
        if (!updateResult.IsSuccess)
            return ValueOrResult.Failure(updateResult.ErrorMessage!);

        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
