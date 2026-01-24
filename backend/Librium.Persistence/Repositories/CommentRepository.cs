using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence.Repositories;
public class CommentRepository : ICommentRepository
{
    private readonly LibriumDbContext _context;
    public CommentRepository(LibriumDbContext context)
    {
        _context = context;
    }
    public void Add(Comment comment)
    {
        _context.Comments.Add(comment);
    }

    public async Task<IReadOnlyList<Comment>> GetByBookIdAsync(Guid bookId)
    {
        return await _context.Comments
            .Where(c => c.BookId == bookId)
            .OrderByDescending(c => c.CreatedAt)
            .Include(c => c.Reactions)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid commentId)
    {
        return await _context.Comments
            .Include(c => c.Reactions)
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
