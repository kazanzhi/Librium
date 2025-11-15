using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.BookCategories;
using Librium.Domain.Common;

namespace Librium.Application.Interfaces;
public interface IBookCategoryService
{
    Task<List<BookCategoryResponseDto>> GetAllBookCategoriesAsync();
    Task<BookCategoryResponseDto> GetBookCategoryById(int categoryId);
    Task<ValueOrResult<int>> AddBookCategoryAsync(BookCategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(int categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto);
}
