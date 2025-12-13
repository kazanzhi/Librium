using Librium.Domain.Books.Models;
using Librium.Domain.Books.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class BookCategoryRepository : IBookCategoryRepository
{
    private readonly LibriumDbContext _context;

    public BookCategoryRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public async Task<BookCategory> AddBookCategory(BookCategory category)
    {
        var result = await _context.BookCategories.AddAsync(category);

        return result.Entity;
    }

    public void Delete(BookCategory entity)
    {
        _context.BookCategories.Remove(entity);
    }

    public async Task<bool> ExistByNameAsync(string name)
    {
        return await _context.BookCategories
            .AnyAsync(c => c.Name == name);
    }

    public async Task<List<BookCategory>> GetAllBookCategoriesAsync()
    {
        var result = await _context.BookCategories.ToListAsync();
        return result;
    }

    public async Task<BookCategory?> GetBookCategoryByIdAsync(Guid categoryId)
    {
        var result = await _context.BookCategories.FindAsync(categoryId);
        return result;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
