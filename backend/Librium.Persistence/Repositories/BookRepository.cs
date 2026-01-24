using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibriumDbContext _context;

    public BookRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public void Add(Book book)
    {
        _context.Books.Add(book);
    }

    public void Delete(Book entity)
    {
        _context.Books.Remove(entity);
    }

    public async Task<bool> ExistBookAsync(string author, string title)
    {
        return await _context.Books
            .AnyAsync(b => b.Author == author && b.Title == title);
    }

    public async Task<IReadOnlyList<Book>> GetAllBooks(string? search)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(book =>
                book.Title.Contains(search) ||
                book.Author.Contains(search)
            );
        }

        return await query
            .Include(c => c.Categories)
            .ToListAsync();
    }

    public async Task<Book?> GetBookById(Guid bookId)
    {
        return await _context.Books
            .Include(c => c.Categories)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
