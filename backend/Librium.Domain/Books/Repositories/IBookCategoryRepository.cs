using Librium.Domain.Books.Dtos;
using Librium.Domain.Books.Models;

namespace Librium.Domain.Repositories;
public interface IBookCategoryRepository
{
    Task<List<BookCategory>> GetBookCategories();
    Task<BookCategory> GetBookCategory(int categoryId);
    Task<BookCategory> CreateBookCategory(BookCategoryDto categoryDto);
    Task<int> DeleteBookCategory(int categoryId);
    Task<int> UpdateBookCategory(int categoryId, BookCategoryDto categoryDto);
}
