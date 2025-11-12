using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.Entities.Books;

namespace Librium.Domain.Interfaces;
public interface IBookCategoryService
{
    Task<List<BookCategory>> GetAllBookCategoriesAsync();

    Task<ValueOrResult<int>> AddBookCategoryAsync(BookCategoryDto categoryDto);
    Task<ValueOrResult> DeleteBookCategoryAsync(int categoryId);
    Task<ValueOrResult> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto);
}
