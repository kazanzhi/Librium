using Librium.Domain.Repositories;
using Librium.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class UserBookRepository : IUserBookRepository
{
    private readonly LibriumDbContext _context;
    public UserBookRepository(LibriumDbContext context)
    {
        _context = context;
    }
    public async Task<UserBook> AddUserBook(UserBook userBook)
    {
        var result = await _context.UserBooks.AddAsync(userBook);
        return result.Entity;
    }

    public async Task Delete(UserBook entity)
    {
        _context.UserBooks.Remove(entity);
    }

    public async Task<List<UserBook>> GetAllUserBooks(string userId)
    {
        return await _context.UserBooks
            .Where(u => u.UserId == userId)
            .Include(c => c.Book.BookCategory)
            .ToListAsync();
    }

    public async Task<UserBook?> GetUserBookById(string userId, Guid bookId)
    {
        return await _context.UserBooks
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
