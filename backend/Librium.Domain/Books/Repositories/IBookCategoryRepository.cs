using Librium.Domain.Common.Repositories;
using Librium.Domain.Books.Models;

namespace Librium.Domain.Books.Repositories;

public interface IBookCategoryRepository : IBaseRepository<BookCategory>
{
    Task<BookCategory?> GetBookCategoryByIdAsync(Guid categoryId);
    Task<List<BookCategory>> GetAllBookCategoriesAsync();
    Task<BookCategory> AddBookCategory(BookCategory category);
    Task<bool> ExistByNameAsync(string category);
 }
