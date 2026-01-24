namespace Librium.Domain.Categories.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetBookCategoryByIdAsync(Guid categoryId);
    Task<IReadOnlyList<Category>> GetAllBookCategoriesAsync();
    Task<Category?> GetByNameAsync(string category);
    Task<bool> SaveChangesAsync();
    void Add(Category category);
    void Delete(Category category);
 }
