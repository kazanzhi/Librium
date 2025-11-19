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
    public async Task<AppUser?> GetUserWithBooks(string userId)
    {
        return await _context.Users
            .Include(x => x.UserBooks)
            .ThenInclude(x => x.Book)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
