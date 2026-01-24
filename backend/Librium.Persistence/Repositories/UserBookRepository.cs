using Librium.Application.Books.DTOs;
using Librium.Application.Libraries.Repositories;
using Librium.Domain.Books;
using Librium.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class UserBookRepository : IUserBookRepository
{
    private readonly LibriumDbContext _context;
    public UserBookRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public void Add(Guid userId, Guid bookId)
    {
        _context.UserBooks.Add(new UserBook
        {
            UserId = userId,
            BookId = bookId
        });
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid bookId)
    {
        return await _context.UserBooks
            .AnyAsync(x => x.UserId == userId && x.BookId == bookId);
    }

    public async Task<IReadOnlyList<Book>> GetBooksByUserIdAsync(Guid userId)
    {
        return await _context.UserBooks
            .Where(x => x.UserId == userId)
            .Join(
                _context.Books,
                ub => ub.BookId,
                b => b.Id,
                (ub, b) => b
            )
            .Include(c => c.Categories)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Remove(Guid userId, Guid bookId)
    {
        var entity = await _context.UserBooks
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

        if (entity is not null)
            _context.UserBooks.Remove(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
