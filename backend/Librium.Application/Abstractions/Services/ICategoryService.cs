using Librium.Application.DTOs.Categories;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllBookCategoriesAsync();
    Task<CategoryResponseDto> GetBookCategoryById(Guid categoryId);
    Task<ValueOrResult<Guid>> CreateBookCategoryAsync(CategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, CategoryDto categoryDto);
}
