using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.DTOs.BookCategories;

namespace Librium.Domain.Interfaces;

public interface IBookCategoryService
{
    Task<List<BookCategoryResponseDto>> GetAllBookCategoriesAsync();
    Task<BookCategoryResponseDto> GetBookCategoryById(Guid categoryId);
    Task<ValueOrResult<Guid>> AddBookCategoryAsync(BookCategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, BookCategoryDto categoryDto);
}
