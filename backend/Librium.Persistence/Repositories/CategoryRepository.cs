using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LibriumDbContext _context;

    public CategoryRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public async Task<Category> AddBookCategory(Category category)
    {
        var result = await _context.Categories.AddAsync(category);

        return result.Entity;
    }

    public void Delete(Category entity)
    {
        _context.Categories.Remove(entity);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IReadOnlyList<Category>> GetAllBookCategoriesAsync()
    {
        return await _context.Categories
            .ToListAsync();
    }

    public async Task<Category?> GetBookCategoryByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .FindAsync(categoryId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
