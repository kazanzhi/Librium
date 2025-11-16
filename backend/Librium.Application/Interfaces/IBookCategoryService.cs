using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.BookCategories;
using Librium.Domain.Common;

namespace Librium.Application.Interfaces;
public interface IBookCategoryService
{
    Task<List<BookCategoryResponseDto>> GetAllBookCategoriesAsync();
    Task<BookCategoryResponseDto> GetBookCategoryById(Guid categoryId);
    Task<ValueOrResult<Guid>> AddBookCategoryAsync(BookCategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, BookCategoryDto categoryDto);
}
