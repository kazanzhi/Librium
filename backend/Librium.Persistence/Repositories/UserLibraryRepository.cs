using Librium.Domain.Users.Models;
using Librium.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;

public class UserLibraryRepository : IUserLibraryRepository
{
    private readonly LibriumDbContext _context;
    public UserLibraryRepository(LibriumDbContext context)
    {
        _context = context;
    }

    public async Task Add(UserLibrary library)
    {
        await _context.UserLibraries.AddAsync(library);
    }

    public async Task<UserLibrary?> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserLibraries
            .Include(l => l.Books)
            .FirstOrDefaultAsync(l => l.UserId == userId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
