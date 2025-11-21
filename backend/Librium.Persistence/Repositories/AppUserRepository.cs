using Librium.Domain.Users.Models;
using Librium.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly LibriumDbContext _context;
    public AppUserRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public async Task Add(UserBook userBook)
    {
        await _context.UserBooks.AddAsync(userBook);
    }

    public async Task Delete(UserBook userBook)
    {
        _context.UserBooks.Remove(userBook);
    }

    public async Task<List<UserBook>> GetAppUserBooks(string userId)
    {
        return await _context.UserBooks
            .Where(x => x.UserId == userId)
            .Include(x => x.Book)
            .ToListAsync();
    }

    public async Task<AppUser?> GetAppUserById(string userId)
    {
        return await _context.Users
        .Include(u => u.UserBooks)
            .ThenInclude(ub => ub.Book)
        .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
