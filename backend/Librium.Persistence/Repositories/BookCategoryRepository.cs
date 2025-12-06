using Librium.Domain.Entities.Books;
using Librium.Domain.Repositories;
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

    public async Task<List<BookCategory>> GetAllBookCategories()
    {
        var result = await _context.BookCategories.ToListAsync();
        return result;
    }

    public async Task<BookCategory?> GetBookCategoryById(Guid categoryId)
    {
        var result = await _context.BookCategories.FindAsync(categoryId);
        return result;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
