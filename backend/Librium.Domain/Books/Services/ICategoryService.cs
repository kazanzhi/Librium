using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.DTOs.BookCategories;

namespace Librium.Domain.Books.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllBookCategoriesAsync();
    Task<CategoryResponseDto> GetBookCategoryById(Guid categoryId);
    Task<ValueOrResult<Guid>> CreateBookCategoryAsync(CategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, CategoryDto categoryDto);
}
