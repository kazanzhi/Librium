namespace Librium.Domain.Categories.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByIdAsync(Guid categoryId);
    Task<IReadOnlyList<Category>> GetAllCategoriesAsync();
    Task<Category?> GetByNameAsync(string category);
    Task<bool> SaveChangesAsync();
    void Add(Category category);
    void Delete(Category category);
 }
