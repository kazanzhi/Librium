using Librium.Domain.Common.Repositories;
using Librium.Domain.Books.Models;

namespace Librium.Domain.Books.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetBookCategoryByIdAsync(Guid categoryId);
    Task<List<Category>> GetAllBookCategoriesAsync();
    Task<Category> AddBookCategory(Category category);
    Task<Category?> GetByNameAsync(string category);
 }
