using Librium.Domain.Common.Repositories;

namespace Librium.Domain.Categories.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetBookCategoryByIdAsync(Guid categoryId);
    Task<List<Category>> GetAllBookCategoriesAsync();
    Task<Category> AddBookCategory(Category category);
    Task<Category?> GetByNameAsync(string category);
 }
